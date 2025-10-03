using Microsoft.EntityFrameworkCore;
using InventarioBackend.Models; 

namespace InventarioBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para cada uno de tus modelos, que representarán las tablas en la base de datos
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<InventarioItem> InventarioItems { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<SalidaItem> SalidaItems { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para evitar multiple cascade paths
            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.Inventario)
                .WithMany(i => i.Movimientos)
                .HasForeignKey(m => m.IdInventario)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.InventarioItem)
                .WithMany(ii => ii.Movimientos)
                .HasForeignKey(m => m.IdInventarioItem)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Salida>(entity =>
            {
                entity.ToTable("Salidas");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Usuario)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Estado)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            modelBuilder.Entity<SalidaItem>(entity =>
            {
                entity.ToTable("SalidaItems");
                entity.HasKey(si => si.Id);

                entity.HasOne(si => si.Salida)
                      .WithMany(s => s.Items)
                      .HasForeignKey(si => si.IdSalida);

                entity.HasOne(si => si.InventarioItem)
                      .WithMany() 
                      .HasForeignKey(si => si.IdInventarioItem);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
