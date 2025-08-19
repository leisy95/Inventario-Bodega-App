using InventarioBackend.Data; // aquí debe estar tu DbContext
using InventarioBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventarioBackend.Services
{
    public class InventarioService
    {
        private readonly ApplicationDbContext _context;

        public InventarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todo el inventario
        public async Task<List<Inventario>> GetInventarioAsync()
        {
            return await _context.Inventarios.ToListAsync();
        }

        // Obtener un item por ID
        public async Task<Inventario> GetInventarioByIdAsync(int id)
        {
            return await _context.Inventarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        // Crear un nuevo item
        public async Task<Inventario> CreateInventarioAsync(Inventario inventario)
        {
            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();
            return inventario;
        }

        // Actualizar un item
        public async Task<Inventario> UpdateInventarioAsync(Inventario inventario)
        {
            _context.Inventarios.Update(inventario);
            await _context.SaveChangesAsync();
            return inventario;
        }

        // Eliminar un item
        public async Task<bool> DeleteInventarioAsync(int id)
        {
            var item = await _context.Inventarios.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            _context.Inventarios.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
