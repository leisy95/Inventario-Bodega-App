using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioBackend.Data;
using InventarioBackend.Models;
using InventarioBackend.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosInventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovimientosInventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MovimientosInventario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoInventarioResponse>>> GetMovimientos()
        {
            var movimientos = await _context.MovimientosInventario
                .Include(m => m.Inventario)
                .Include(m => m.InventarioItem)
                .Select(m => new MovimientoInventarioResponse
                {
                    Id = m.Id,
                    Referencia = m.Referencia,
                    ReferenciaPeso = m.ReferenciaPeso,
                    Peso = m.Peso,
                    Fecha = m.Fecha,
                    Tipo = m.Tipo,
                    Usuario = m.Usuario,
                    InventarioNombre = m.Inventario.Referencia,       // 🔹 Nombre del inventario
                    InventarioItemCodigo = m.InventarioItem.ReferenciaPeso // 🔹 Código del item
                })
                .ToListAsync();

            return Ok(movimientos);
        }

        // GET: api/MovimientosInventario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoInventarioResponse>> GetMovimiento(int id)
        {
            var movimiento = await _context.MovimientosInventario
                .Include(m => m.Inventario)
                .Include(m => m.InventarioItem)
                .Where(m => m.Id == id)
                .Select(m => new MovimientoInventarioResponse
                {
                    Id = m.Id,
                    Referencia = m.Referencia,
                    ReferenciaPeso = m.ReferenciaPeso,
                    Peso = m.Peso,
                    Fecha = m.Fecha,
                    Tipo = m.Tipo,
                    Usuario = m.Usuario,
                    InventarioNombre = m.Inventario.Referencia,
                    InventarioItemCodigo = m.InventarioItem.ReferenciaPeso
                })
                .FirstOrDefaultAsync();

            if (movimiento == null)
                return NotFound();

            return Ok(movimiento);
        }

        // Items inicial auditoria
        [HttpGet("resumen")]
        public async Task<ActionResult<object>> GetResumen()
        {
            var inventarioInicial = await _context.MovimientosInventario
                .Where(m => m.Tipo == "Inicial")
                .SumAsync(m => m.Peso);

            var entradas = await _context.MovimientosInventario
                .Where(m => m.Tipo == "Entrada")
                .SumAsync(m => m.Peso);

            var salidas = await _context.MovimientosInventario
                .Where(m => m.Tipo == "Salida")
                .SumAsync(m => m.Peso);

            var ajustes = await _context.MovimientosInventario
                .Where(m => m.Tipo == "Ajuste")
                .SumAsync(m => m.Peso);

            var existencias = inventarioInicial + entradas - salidas + ajustes;

            var resumen = new
            {
                InventarioInicial = inventarioInicial,
                Entradas = entradas,
                Salidas = salidas,
                Ajustes = ajustes,
                Existencias = existencias
            };

            return Ok(resumen);
        }

        // MOSTRAR DATOS POR RANGOS 
        [HttpGet("auditoria")]
        public async Task<IActionResult> GetAuditoria([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var query = _context.MovimientosInventario.AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(m => m.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(m => m.Fecha <= fechaFin.Value);

            var auditoria = new
            {
                TotalesPorTipo = await query
                    .GroupBy(m => m.Tipo)
                    .Select(g => new { Tipo = g.Key, Total = g.Count() })
                    .ToListAsync(),

                ReferenciasMasMovidas = await query
                    .GroupBy(m => m.Referencia)
                    .Select(g => new { Referencia = g.Key, Total = g.Count() })
                    .OrderByDescending(x => x.Total)
                    .Take(5)
                    .ToListAsync(),

                UsuariosMasActivos = await query
                    .GroupBy(m => m.Usuario)
                    .Select(g => new { Usuario = g.Key, Total = g.Count() })
                    .OrderByDescending(x => x.Total)
                    .Take(5)
                    .ToListAsync()
            };

            return Ok(auditoria);
        }

        // POST: api/MovimientosInventario
        [HttpPost]
        public async Task<ActionResult<MovimientoInventarioResponse>> PostMovimiento(MovimientoInventarioRequest request)
        {
            var movimiento = new MovimientoInventario
            {
                Referencia = request.Referencia,
                ReferenciaPeso = request.ReferenciaPeso,
                Peso = request.Peso,
                Fecha = request.Fecha,
                Tipo = request.Tipo,
                Usuario = request.Usuario,
                IdInventario = request.IdInventario,
                IdInventarioItem = request.IdInventarioItem
            };

            _context.MovimientosInventario.Add(movimiento);
            await _context.SaveChangesAsync();

            // Mapear a response
            var response = await _context.MovimientosInventario
                .Include(m => m.Inventario)
                .Include(m => m.InventarioItem)
                .Where(m => m.Id == movimiento.Id)
                .Select(m => new MovimientoInventarioResponse
                {
                    Id = m.Id,
                    Referencia = m.Referencia,
                    ReferenciaPeso = m.ReferenciaPeso,
                    Peso = m.Peso,
                    Fecha = m.Fecha,
                    Tipo = m.Tipo,
                    Usuario = m.Usuario,
                    InventarioNombre = m.Inventario.Referencia,
                    InventarioItemCodigo = m.InventarioItem.ReferenciaPeso
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.Id }, response);
        }
    }
}