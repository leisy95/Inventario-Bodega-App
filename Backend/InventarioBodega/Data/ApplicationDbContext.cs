using Microsoft.EntityFrameworkCore;
using InventarioBackend.Models; // Asegúrate de incluir esta importación

namespace InventarioBackend.Data
{
    // ApplicationDbContext hereda de DbContext, que es la clase base de Entity Framework Core
    public class ApplicationDbContext : DbContext
    {
        // Constructor que recibe DbContextOptions, necesario para la configuración
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para cada uno de tus modelos, que representarán las tablas en la base de datos
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<TipoProducto> TiposProducto { get; set; }
        public DbSet<InventarioItem> InventarioItems { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }

        // Opcional: Puedes sobrescribir OnModelCreating para configurar el modelo
        // Esto es útil para relaciones más complejas, índices, o para definir
        // el comportamiento de eliminación en cascada.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ejemplo de configuración si fuera necesario (no estrictamente necesario para este caso simple)
            // modelBuilder.Entity<InventarioItem>()
            //     .HasOne(ii => ii.TipoProducto) // InventarioItem tiene un TipoProducto
            //     .WithMany() // Un TipoProducto puede tener muchos InventarioItems
            //     .HasForeignKey(ii => ii.IdTipoProducto); // La clave foránea es IdTipoProducto

            // Asegúrate de llamar al método base
            base.OnModelCreating(modelBuilder);
        }
    }
}
