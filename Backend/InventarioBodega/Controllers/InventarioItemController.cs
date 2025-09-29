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
                .OrderBy(i => i.FechaRegistroItem) // FIFO: más antiguo primero
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
                    Usuario = User.Identity?.Name ?? "Sistema", // Mejor si usas JWT
                    IdInventario = inventario.Id,
                    IdInventarioItem = item.Id
                };

                _context.MovimientosInventario.Add(movimiento);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"{items.Count} items dados de salida correctamente." });
        }

        [HttpPut("update-peso/{id}")]
        public async Task<IActionResult> UpdatePeso(int id, [FromBody] EditarPesoRequest request)
        {
            if (request == null)
                return BadRequest("Datos inválidos.");

            var item = await _context.InventarioItems.FindAsync(id);
            if (item == null)
                return NotFound("Item no encontrado.");

            if (item.Estado != "EN_ALMACEN")
                return BadRequest("Solo se pueden editar items con estado EN_ALMACEN.");

            var inventario = await _context.Inventarios.FindAsync(item.IdInventario);
            if (inventario == null)
                return NotFound("Inventario general no encontrado.");

            var pesoAntiguo = item.PesoActual;
            var diferenciaPeso = request.PesoActual - pesoAntiguo;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Actualizar item
                item.PesoActual = request.PesoActual;

                // Ajustar inventario general
                inventario.Peso = Math.Max(0, inventario.Peso + diferenciaPeso);

                // Registrar movimiento histórico
                var movimiento = new MovimientoInventario
                {
                    Referencia = inventario.ReferenciaNormalizada,
                    ReferenciaPeso = item.ReferenciaPeso,
                    Peso = request.PesoActual,
                    Fecha = DateTime.UtcNow,
                    Tipo = $"Modificación Peso ({pesoAntiguo} → {request.PesoActual})",
                    Usuario = User?.Identity?.Name ?? "Sistema",
                    IdInventario = inventario.Id,
                    IdInventarioItem = item.Id
                };
                _context.MovimientosInventario.Add(movimiento);

                // Guardar cambios
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Peso actualizado, inventario ajustado y movimiento registrado" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
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

                inventario.Peso = Math.Max(0, inventario.Peso - item.PesoActual);

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
