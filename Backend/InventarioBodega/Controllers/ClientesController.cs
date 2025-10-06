
using InventarioBackend.Data;
using InventarioBackend.DTOs;
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
        public async Task<IActionResult> CrearCliente([FromBody] ClienteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Cliente
            {
                Nombre = request.Nombre,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Email = request.Email
            };

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
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] EditarClienteRequest request)
        {
            if (request.Id != id)
                return BadRequest(new { message = "El ID en la URL no coincide con el ID del cliente" });

            var clienteDb = await _context.Clientes.FindAsync(id);
            if (clienteDb == null)
                return NotFound(new { message = "Cliente no encontrado" });

            // Actualizamos solo los campos permitidos
            clienteDb.Nombre = request.Nombre;
            clienteDb.Direccion = request.Direccion;
            clienteDb.Telefono = request.Telefono;
            clienteDb.Email = request.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al actualizar cliente", error = ex.Message });
            }

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