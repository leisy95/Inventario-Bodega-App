using Microsoft.EntityFrameworkCore;
using InventarioBackend.Models; 

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
        //public DbSet<TipoProducto> TiposProducto { get; set; }
        public DbSet<InventarioItem> InventarioItems { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para evitar multiple cascade paths
            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.Inventario)
                .WithMany(i => i.Movimientos)
                .HasForeignKey(m => m.IdInventario)
                .OnDelete(DeleteBehavior.Restrict); // 👈 usa Restrict para mayor claridad

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.InventarioItem)
                .WithMany(ii => ii.Movimientos)
                .HasForeignKey(m => m.IdInventarioItem)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
