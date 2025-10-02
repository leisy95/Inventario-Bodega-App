using InventarioBackend.Data;
using InventarioBackend.DTOs;
using InventarioBackend.Helpers;
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
            var refNormalizada = ReferenciaHelper.Normalize(referencia);

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

            // Normalizar la referencia
            var refNormalizada = ReferenciaHelper.Normalize(input.Referencia);

            var producto = await _context.Inventarios
                                 .FirstOrDefaultAsync(p => p.ReferenciaNormalizada == refNormalizada);

            if (producto == null)
                return NotFound("Referencia no encontrada");

            // Generar nueva referencia con peso individual
            var pesoFormateado = input.Peso.ToString("0.##").Replace(".", "").Replace(",", "");
            var referenciaConPeso = $"{producto.ReferenciaNormalizada}{pesoFormateado}";

            // Crear un nuevo InventarioItem siempre
            var nuevoItem = new InventarioItem
            {
                IdInventario = producto.Id,
                PesoActual = input.Peso,
                FechaRegistroItem = DateTime.Now,
                Estado = "EN_ALMACEN",
                ReferenciaPeso = referenciaConPeso
            };

            _context.InventarioItems.Add(nuevoItem);

            // Actualizar Inventario
            producto.Peso += input.Peso;
            producto.Cantidad += 1;

            // Guardar primero para obtener el Id del nuevo item
            await _context.SaveChangesAsync();

            // Crear registro histórico de movimiento
            var nuevoMovimiento = new MovimientoInventario
            {
                Referencia = producto.ReferenciaNormalizada,
                ReferenciaPeso = nuevoItem.ReferenciaPeso,
                Peso = input.Peso,
                Fecha = DateTime.Now,
                Tipo = "Entrada",
                Usuario = User?.Identity?.Name ?? "Sistema",
                IdInventario = producto.Id,
                IdInventarioItem = nuevoItem.Id
            };

            _context.MovimientosInventario.Add(nuevoMovimiento);
            await _context.SaveChangesAsync();

            // Devolver la respuesta
            var response = new IngresarPesoResponse
            {
                Referencia = producto.Referencia,
                ReferenciaPeso = nuevoItem.ReferenciaPeso,
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

                    // Siempre inicializados en 0 aquí
                    Peso = 0,
                    Cantidad = 0,

                    ReferenciaNormalizada = ReferenciaHelper.Normalize(request.Referencia)
                };

                _context.Inventarios.Add(inventario);
                await _context.SaveChangesAsync();

                return Ok(new RegistrarItemResponse
                {
                    IdItemGenerado = inventario.Id,
                    Referencia = inventario.Referencia,
                    Message = "Producto registrado correctamente en Inventario"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error interno", Error = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarInventario(int id, Inventario inventario)
        {
            if (id != inventario.Id)
                return BadRequest(new { mensaje = "El ID no coincide" });

            _context.Entry(inventario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Inventarios.Any(e => e.Id == id))
                    return NotFound(new { mensaje = "Inventario no encontrado" });
                else
                    throw;
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el inventario", detalle = ex.Message });
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