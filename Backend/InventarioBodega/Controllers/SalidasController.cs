using InventarioBackend.Data;
using InventarioBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalidasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalidasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Usuario fijo mientras no haya JWT/login
        private string UsuarioActual => "Sistema";

        // Obtener la salida actual en proceso
        [HttpGet("actual")]
        public async Task<IActionResult> ObtenerSalidaActual()
        {
            var salida = await _context.Salidas
                .Include(s => s.Items)
                .ThenInclude(si => si.InventarioItem)
                .FirstOrDefaultAsync(s => s.Usuario == UsuarioActual && s.Estado == "EN_PROCESO");

            if (salida == null)
                return NotFound("No hay salida en proceso.");

            return Ok(new
            {
                salida.Id,
                salida.Estado,
                salida.FechaCreacion,
                Items = salida.Items.Select(i => new
                {
                    i.IdInventarioItem,
                    i.InventarioItem.ReferenciaPeso,
                    i.InventarioItem.PesoActual
                }).ToList()
            });
        }

        // Crear nueva salida en proceso
        [HttpPost("crear")]
        public async Task<IActionResult> CrearSalida()
        {
            var salidaExistente = await _context.Salidas
                .FirstOrDefaultAsync(s => s.Usuario == UsuarioActual && s.Estado == "EN_PROCESO");

            if (salidaExistente != null)
                return BadRequest(new { message = "Ya tienes una salida en proceso." });

            var salida = new Salida
            {
                Usuario = UsuarioActual,
                Estado = "EN_PROCESO",
                FechaCreacion = DateTime.Now
            };

            _context.Salidas.Add(salida);
            await _context.SaveChangesAsync();

            return Ok(salida);
        }

        // Agregar item a la salida
        [HttpPost("{idSalida}/agregar-item/{idItem}")]
        public async Task<IActionResult> AgregarItem(int idSalida, int idItem)
        {
            var salida = await _context.Salidas
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == idSalida && s.Estado == "EN_PROCESO");

            if (salida == null)
                return BadRequest(new { message = "Salida no encontrada o ya finalizada." });

            // Buscar el item que esté en ALMACEN o ya RESERVADO
            var item = await _context.InventarioItems
                .FirstOrDefaultAsync(i => i.Id == idItem &&
                                          (i.Estado == "EN_ALMACEN" || i.Estado == "RESERVADO"));

            if (item == null)
                return BadRequest(new { message = "El item no está disponible en almacén." });

            // Verificar si ya está reservado en otra salida
            bool reservadoEnOtraSalida = await _context.SalidaItems
                .AnyAsync(si => si.IdInventarioItem == idItem
                             && si.Salida.Estado == "EN_PROCESO"
                             && si.IdSalida != idSalida);

            if (reservadoEnOtraSalida)
                return BadRequest(new { message = "El item ya está reservado en otra salida." });

            // Verificar si ya estaba agregado en ESTA salida
            bool yaAgregado = salida.Items.Any(si => si.IdInventarioItem == idItem);
            if (yaAgregado)
                return Ok(new
                {
                    message = "El item ya estaba en esta salida.",
                    item.Id,
                    item.ReferenciaPeso,
                    item.PesoActual
                });

            // Reservar el item
            item.Estado = "RESERVADO";

            var salidaItem = new SalidaItem
            {
                IdSalida = salida.Id,
                IdInventarioItem = item.Id,
                InventarioItem = item
            };

            _context.SalidaItems.Add(salidaItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Item agregado a la salida.",
                item.Id,
                item.ReferenciaPeso,
                item.PesoActual
            });
        }

        // Quitar item de la salida
        [HttpDelete("{idSalida}/quitar-item/{idItem}")]
        public async Task<IActionResult> QuitarItem(int idSalida, int idItem)
        {
            var salidaItem = await _context.SalidaItems
                .Include(si => si.InventarioItem)
                .FirstOrDefaultAsync(si => si.IdSalida == idSalida && si.IdInventarioItem == idItem);

            if (salidaItem == null)
                return NotFound(new { message = "El item no está en la salida." });

            // Liberar el item
            salidaItem.InventarioItem.Estado = "EN_ALMACEN";

            _context.SalidaItems.Remove(salidaItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item eliminado de la salida." });
        }

        // Confirmar salida
        [HttpPost("{idSalida}/confirmar")]
        public async Task<IActionResult> ConfirmarSalida(int idSalida)
        {
            var salida = await _context.Salidas
                .Include(s => s.Items)
                .ThenInclude(si => si.InventarioItem)
                .FirstOrDefaultAsync(s => s.Id == idSalida && s.Estado == "EN_PROCESO");

            if (salida == null)
                return BadRequest(new { message = "No se encontró la salida o ya fue confirmada." });

            foreach (var si in salida.Items)
            {
                var item = si.InventarioItem;
                if (item.Estado != "RESERVADO") continue;

                // Cambiar estado a SALIDA
                item.Estado = "SALIDA";

                // Buscar inventario base
                var inventario = await _context.Inventarios.FindAsync(item.IdInventario);

                // Registrar movimiento
                var movimiento = new MovimientoInventario
                {
                    Referencia = inventario?.ReferenciaNormalizada ?? item.ReferenciaPeso,
                    ReferenciaPeso = item.ReferenciaPeso,
                    Peso = item.PesoActual,
                    Fecha = DateTime.Now,
                    Tipo = "Salida",
                    Usuario = UsuarioActual,
                    IdInventario = item.IdInventario,
                    IdInventarioItem = item.Id
                };

                _context.MovimientosInventario.Add(movimiento);

                // Restar peso y cantidad al inventario
                if (inventario != null)
                {
                    inventario.Peso = Math.Max(0, inventario.Peso - item.PesoActual);
                    inventario.Cantidad = Math.Max(0, inventario.Cantidad - 1);
                }
            }

            // Actualizar estado de la salida
            salida.Estado = "CONFIRMADA";
            salida.FechaSalida = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Salida confirmada con éxito." });
        }

        // Cancelar salida
        [HttpPost("{idSalida}/cancelar")]
        public async Task<IActionResult> CancelarSalida(int idSalida)
        {
            var salida = await _context.Salidas
                .Include(s => s.Items)
                .ThenInclude(si => si.InventarioItem)
                .FirstOrDefaultAsync(s => s.Id == idSalida && s.Estado == "EN_PROCESO");

            if (salida == null)
                return BadRequest(new { message = "No se encontró la salida o ya fue finalizada." });

            foreach (var si in salida.Items)
            {
                si.InventarioItem.Estado = "EN_ALMACEN";
            }

            salida.Estado = "CANCELADA";
            salida.FechaCancelacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Salida cancelada." });
        }
    }
}