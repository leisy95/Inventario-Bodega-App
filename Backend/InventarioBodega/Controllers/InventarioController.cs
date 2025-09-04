using InventarioBackend.Data;
using InventarioBackend.DTOs;
using InventarioBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrarItemRequest = InventarioBackend.DTOs.RegistrarItemRequest;
using RegistrarItemResponse = InventarioBackend.DTOs.RegistrarItemResponse;

namespace InventarioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/inventario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventario>>> GetInventarios()
        {
            return await _context.Inventarios.ToListAsync();
        }

        // GET: api/inventario
        [HttpGet("buscar/{referencia}")]
        public async Task<ActionResult<Inventario>> GetByReferencia(string referencia)
        {
            var refNormalizada = referencia.Replace(".", "").Replace("*", "");

            // Buscar en la columna indexada
            var inventario = await _context.Inventarios
                .FirstOrDefaultAsync(i => i.ReferenciaNormalizada == refNormalizada);

            if (inventario == null)
                return NotFound(new { mensaje = "Referencia no encontrada" });

            return Ok(inventario);
        }

        [HttpPost("ingresar-peso")]
        public async Task<IActionResult> IngresarPeso([FromBody] IngresarPesoRequest input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var refNormalizada = input.Referencia.Replace(".", "").Replace("*", "");

            var producto = await _context.Inventarios
                                 .FirstOrDefaultAsync(p => p.ReferenciaNormalizada == refNormalizada);

            if (producto == null)
                return NotFound("Referencia no encontrada");

            // Sumar peso y cantidad
            producto.Peso += input.Peso;
            producto.Cantidad += 1;

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolver la respuesta
            var response = new IngresarPesoResponse
            {
                Referencia = producto.Referencia,
                Peso = producto.Peso,
                Cantidad = producto.Cantidad,
                Descripcion = producto.Descripcion,
                Ancho = producto.Ancho,
                Alto = producto.Alto,
                Calibre = producto.Calibre,
                Color = producto.Color,
                Material = producto.TipoMaterial
            };

            return Ok(response);
        }

        [HttpPost("registrar-item")]
        public async Task<ActionResult<RegistrarItemResponse>> RegistrarItem([FromBody] RegistrarItemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var inventario = new Inventario
                {
                    Referencia = request.Referencia,
                    Descripcion = request.Descripcion,
                    TipoBolsa = request.TipoBolsa,
                    TipoMaterial = request.TipoMaterial,
                    Densidad = request.Densidad,
                    Color = request.Color,
                    SegundoColor = request.SegundoColor,
                    ImpresoNo = request.ImpresoNo,
                    Ancho = request.Ancho ?? 0,
                    Alto = request.Alto ?? 0,
                    Calibre = request.Calibre ?? 0,

                    // 🚨 Siempre inicializados en 0 aquí
                    Peso = 0,
                    Cantidad = 0,

                    ReferenciaNormalizada = request.Referencia?
                           .Replace(".", "")
                           .Replace("*", "")
                           .ToUpper()
                };

                _context.Inventarios.Add(inventario);
                await _context.SaveChangesAsync();

                // También insertamos el InventarioItem asociado
                var inventarioItem = new InventarioItem
                {
                    IdInventario = inventario.Id,
                    FechaRegistroItem = DateTime.UtcNow,
                    Estado = "REGISTRADO",
                    PesoActual = 0
                };

                _context.InventarioItems.Add(inventarioItem);
                await _context.SaveChangesAsync();

                return Ok(new RegistrarItemResponse
                {
                    IdItemGenerado = inventarioItem.Id,
                    Referencia = inventario.Referencia,
                    Message = "Item registrado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error interno", Error = ex.Message });
            }

        }

        // PUT: api/inventario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventario(int id, Inventario inventario)
        {
            if (id != inventario.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Inventarios.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/inventario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventario(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }

            _context.Inventarios.Remove(inventario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}