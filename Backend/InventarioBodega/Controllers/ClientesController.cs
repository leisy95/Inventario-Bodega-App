
using InventarioBackend.Data;
using InventarioBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Crear cliente
        [HttpPost("crear")]
        public async Task<IActionResult> CrearCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente creado con éxito", cliente.Id });
        }

        // Listar todos los clientes
        [HttpGet("listar")]
        public async Task<IActionResult> ListarClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        // Obtener cliente por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = "Cliente no encontrado" });

            return Ok(cliente);
        }

        // (Opcional) Actualizar cliente
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            var clienteDb = await _context.Clientes.FindAsync(id);
            if (clienteDb == null)
                return NotFound(new { message = "Cliente no encontrado" });

            clienteDb.Nombre = cliente.Nombre;
            clienteDb.Direccion = cliente.Direccion;
            clienteDb.Telefono = cliente.Telefono;
            clienteDb.Email = cliente.Email;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cliente actualizado con éxito" });
        }

        // (Opcional) Eliminar cliente
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = "Cliente no encontrado" });

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cliente eliminado con éxito" });
        }
    }
}