using InventarioBackend.Data;
using InventarioBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventarioItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("all-items")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _context.InventarioItems
                .Select(i => new
                {
                    i.Id,
                    i.IdInventario,
                    i.ReferenciaPeso,
                    i.PesoActual,
                    i.FechaRegistroItem,
                    i.Estado
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("No hay items en inventario.");

            return Ok(items);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarItems([FromQuery] string referencia)
        {
            if (string.IsNullOrWhiteSpace(referencia))
                return BadRequest("Debe ingresar una referencia para buscar.");

            var estadosValidos = new[] { "INGRESADO", "EN_ALMACEN", "Activo" };

            var items = await _context.InventarioItems
                .Where(i => i.ReferenciaPeso.Contains(referencia)
                         && estadosValidos.Contains(i.Estado))
                .Select(i => new {
                    i.Id,
                    i.ReferenciaPeso,
                    i.PesoActual,
                    i.Estado,
                    i.FechaRegistroItem
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("No se encontraron coincidencias.");

            return Ok(items);
        }

        [HttpGet("buscar-fifo")]
        public async Task<IActionResult> BuscarFIFO([FromQuery] string referencia)
        {
            var item = await _context.InventarioItems
                .Where(i => i.ReferenciaPeso == referencia && i.Estado == "EN_ALMACEN")
                .OrderBy(i => i.FechaRegistroItem) // más antiguo primero
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound("No hay más items disponibles con esa referencia.");

            return Ok(new
            {
                item.Id,
                item.ReferenciaPeso,
                item.PesoActual,
                item.FechaRegistroItem
            });
        }

        // Registrar salidas múltiples
        [HttpPost("salidas")]
        public async Task<IActionResult> DarSalidas([FromBody] List<string> referenciasPeso)
        {
            if (referenciasPeso == null || !referenciasPeso.Any())
                return BadRequest("No se recibieron referencias.");

            foreach (var refPeso in referenciasPeso)
            {
                var item = await _context.InventarioItems
                    .FirstOrDefaultAsync(i => i.ReferenciaPeso == refPeso && i.Estado == "EN_ALMACEN");

                if (item == null) continue; // ignorar los que no existan o ya salieron

                var inventario = await _context.Inventarios.FindAsync(item.IdInventario);
                if (inventario == null) continue;

                // Restar peso al inventario general
                inventario.Peso = Math.Max(0, inventario.Peso - item.PesoActual);

                // Cambiar estado del item
                item.Estado = "SALIDA";

                // Registrar movimiento
                var movimiento = new MovimientoInventario
                {
                    Referencia = inventario.ReferenciaNormalizada,
                    ReferenciaPeso = item.ReferenciaPeso,
                    Peso = item.PesoActual,
                    Fecha = DateTime.Now,
                    Tipo = "Salida",
                    Usuario = "Sistema" // Aquí luego pones el usuario autenticado (JWT)
                    ,
                    IdInventario = inventario.Id,
                    IdInventarioItem = item.Id
                };

                _context.MovimientosInventario.Add(movimiento);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Salidas registradas con éxito" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.InventarioItems.FindAsync(id);
            if (item == null)
                return NotFound("Item no encontrado.");

            if (item.Estado != "EN_ALMACEN")
                return BadRequest("Solo se pueden eliminar registros con estado EN_ALMACEN.");

            // Usar transacción para mantener consistencia
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Ajustar peso del inventario general (si existe)
                var inventario = await _context.Inventarios.FindAsync(item.IdInventario);
                if (inventario != null)
                {
                    inventario.Peso = Math.Max(0, inventario.Peso - item.PesoActual);
                }

                // Registrar movimiento en histórico
                var movimiento = new MovimientoInventario
                {
                    Referencia = inventario.ReferenciaNormalizada,
                    ReferenciaPeso = item.ReferenciaPeso,
                    Peso = item.PesoActual,
                    Fecha = DateTime.UtcNow,
                    Tipo = "Eliminación",
                    Usuario = User?.Identity?.Name ?? "Sistema", // sustituye por el usuario real si usas JWT
                    IdInventario = inventario?.Id ?? 0,
                    IdInventarioItem = item.Id
                };
                _context.MovimientosInventario.Add(movimiento);

                // Soft delete: marcar como eliminado en vez de remover
                item.Estado = "ELIMINADO";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Item marcado como eliminado y movimiento registrado" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Aquí puedes loguear el error (ex) según tu logger
                return StatusCode(500, "Error interno al intentar eliminar el item.");
            }
        }
    }

}   
