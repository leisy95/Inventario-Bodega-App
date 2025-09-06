using InventarioBackend.Data;
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

        [HttpGet("by-inventario/{idInventario}")]
        public async Task<IActionResult> GetByInventario(int idInventario)
        {
            var items = await _context.InventarioItems
                .Where(i => i.IdInventario == idInventario)
                .Select(i => new
                {
                    i.Id,
                    i.ReferenciaPeso,
                    i.PesoActual,
                    i.FechaRegistroItem,
                    i.Estado
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("No se encontraron items para este inventario");

            return Ok(items);
        }
    }
}
