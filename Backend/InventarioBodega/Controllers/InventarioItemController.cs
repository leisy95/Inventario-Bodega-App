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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] EditarPesoRequest request)
        {
            if (request == null)
                return BadRequest("Body vacío o inválido.");

            var item = await _context.InventarioItems.FindAsync(id);
            if (item == null)
                return NotFound($"No se encontró el item con Id={id}");

            var inventario = await _context.Inventarios.FindAsync(item.IdInventario);
            if (inventario == null)
                return NotFound($"No se encontró el inventario relacionado con Id={item.IdInventario}");

            // --- Ajustar peso acumulado ---
            inventario.Peso -= item.PesoActual;   // quitar peso viejo
            item.PesoActual = request.PesoActual; // actualizar item
            inventario.Peso += request.PesoActual; // sumar peso nuevo

            // --- Regenerar referencia del item ---
            item.ReferenciaPeso = $"{inventario.ReferenciaNormalizada}{request.PesoActual.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                item.Id,
                item.ReferenciaPeso,
                item.PesoActual,
                inventario.Peso,
                inventario.ReferenciaNormalizada
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.InventarioItems.FindAsync(id);
            if (item == null) return NotFound();
            _context.InventarioItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
