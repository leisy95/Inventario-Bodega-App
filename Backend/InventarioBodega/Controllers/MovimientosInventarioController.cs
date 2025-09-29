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

        // MOSTRAR DATOS POR RANGOS 
        [HttpGet("auditoria")]
        public async Task<IActionResult> GetAuditoria([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var query = _context.MovimientosInventario.AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(m => m.Fecha >= fechaInicio.Value.Date); // inicio del día

            if (fechaFin.HasValue)
                query = query.Where(m => m.Fecha <= fechaFin.Value.Date.AddDays(1).AddTicks(-1)); // fin del día

            // Traemos todos los movimientos filtrados
            var movimientosList = await query
                .Select(m => new
                {
                    referencia = m.Referencia,
                    referenciaPeso = m.ReferenciaPeso,
                    tipo = m.Tipo,
                    peso = m.Peso,
                    fecha = m.Fecha,
                    usuario = m.Usuario
                })
                .OrderByDescending(m => m.fecha)
                .ToListAsync();

            // Resumen de inventario
            var inventarioInicial = movimientosList.Where(m => m.tipo == "Inicial").ToList();
            var entradas = movimientosList.Where(m => m.tipo == "Entrada").ToList();
            var salidas = movimientosList.Where(m => m.tipo == "Salida").ToList();

            var resumen = new
            {
                InventarioInicial = new { Cantidad = inventarioInicial.Count, Peso = inventarioInicial.Sum(x => x.peso) },
                Entradas = new { Cantidad = entradas.Count, Peso = entradas.Sum(x => x.peso) },
                Salidas = new { Cantidad = salidas.Count, Peso = salidas.Sum(x => x.peso) },
                Existencias = new
                {
                    Cantidad = inventarioInicial.Count + entradas.Count - salidas.Count,
                    Peso = inventarioInicial.Sum(x => x.peso) + entradas.Sum(x => x.peso) - salidas.Sum(x => x.peso)
                }
            };

            // Totales por tipo
            var totalesPorTipo = movimientosList
                .GroupBy(m => m.tipo)
                .Select(g => new { tipo = g.Key, total = g.Count() })
                .OrderByDescending(x => x.total)
                .Take(5)
                .ToList();

            // Referencias más movidas
            var referenciasMasMovidas = movimientosList
                .GroupBy(m => m.referencia)
                .Select(g => new { referencia = g.Key, total = g.Count() })
                .OrderByDescending(x => x.total)
                .Take(5)
                .ToList();

            // Usuarios más activos
            var usuariosMasActivos = movimientosList
                .GroupBy(m => m.usuario)
                .Select(g => new { usuario = g.Key, total = g.Count() })
                .OrderByDescending(x => x.total)
                .Take(5)
                .ToList();

            // Respuesta combinada
            var auditoria = new
            {
                resumen,
                movimientos = movimientosList,
                totalesPorTipo,
                referenciasMasMovidas,
                usuariosMasActivos
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