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

        [HttpGet("entradas-almacen")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _context.InventarioItems
                .Where(i => i.Estado == "EN_ALMACEN")
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
            if (string.IsNullOrWhiteSpace(referencia))
                return BadRequest(new { message = "Debe enviar una referencia válida." });

            // Buscar el primer item disponible con esa referencia (FIFO)
            var item = await _context.InventarioItems
                .Where(i => i.ReferenciaPeso.StartsWith(referencia) && i.Estado == "EN_ALMACEN")
                .OrderBy(i => i.FechaRegistroItem) 
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "No hay más items disponibles con esa referencia." });

            // Reservar el item para que no lo tome otro usuario
            item.Estado = "RESERVADO";
            await _context.SaveChangesAsync();

            // También devolvemos cuántos quedan disponibles aún EN_ALMACEN
            var disponibles = await _context.InventarioItems
                .CountAsync(i => i.ReferenciaPeso.StartsWith(referencia) && i.Estado == "EN_ALMACEN");

            return Ok(new
            {
                id = item.Id,
                referenciaPeso = item.ReferenciaPeso,
                pesoActual = item.PesoActual,
                fechaRegistroItem = item.FechaRegistroItem,
                disponibles
            });
        }

        // Registrar salidas múltiples
        [HttpPost("salidas")]
        public async Task<IActionResult> DarSalidas([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest(new { message = "Debe enviar al menos un item para salida." });

            var items = await _context.InventarioItems
                .Where(i => ids.Contains(i.Id) && i.Estado == "RESERVADO")
                .ToListAsync();

            if (!items.Any())
                return NotFound(new { message = "No se encontraron items reservados para salida." });

            foreach (var item in items)
            {
                var inventario = await _context.Inventarios.FindAsync(item.IdInventario);
                if (inventario == null) continue;

                // Restar peso del inventario general
                inventario.Peso = Math.Max(0, inventario.Peso - item.PesoActual);

                // Marcar salida
                item.Estado = "SALIDA";

                // Registrar movimiento
                var movimiento = new MovimientoInventario
                {
                    Referencia = inventario.ReferenciaNormalizada,
                    ReferenciaPeso = item.ReferenciaPeso,
                    Peso = item.PesoActual,
                    Fecha = DateTime.Now,
                    Tipo = "Salida",
                    Usuario = User.Identity?.Name ?? "Sistema", 
                    IdInventario = inventario.Id,
                    IdInventarioItem = item.Id
                };

                _context.MovimientosInventario.Add(movimiento);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"{items.Count} items dados de salida correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.InventarioItems.FindAsync(id);
            if (item == null)
                return NotFound("Item no encontrado.");

            if (item.Estado != "EN_ALMACEN")
                return BadRequest("Solo se pueden eliminar registros con estado EN_ALMACEN.");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var inventario = await _context.Inventarios.FindAsync(item.IdInventario);
                if (inventario != null)
                {
                    inventario.Peso = Math.Max(0, inventario.Peso - item.PesoActual);
                    inventario.Cantidad = Math.Max(0, inventario.Cantidad - 1); 
                }

                var movimiento = new MovimientoInventario
                {
                    Referencia = inventario?.ReferenciaNormalizada,
                    ReferenciaPeso = item.ReferenciaPeso,
                    Peso = item.PesoActual,
                    Fecha = DateTime.UtcNow,
                    Tipo = "Eliminado",
                    Usuario = User?.Identity?.Name ?? "Sistema",
                    IdInventario = inventario?.Id ?? 0,
                    IdInventarioItem = item.Id
                };
                _context.MovimientosInventario.Add(movimiento);

                item.Estado = "ELIMINADO";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Item marcado como eliminado y movimiento registrado" });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Error interno al intentar eliminar el item.");
            }
        }
    }

}   
