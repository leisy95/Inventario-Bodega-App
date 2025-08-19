using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InventarioBackend.Models
{
    public class InventarioItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int IdTipoProducto { get; set; } // Clave foránea a TipoProducto

        [ForeignKey("IdTipoProducto")]
        public TipoProducto TipoProducto { get; set; } // Propiedad de navegación

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PesoActual { get; set; }

        [Required]
        public DateTime FechaRegistroItem { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } // Ej: "REGISTRADO", "EN_ALMACEN", "DESPACHADO"
        // Propiedad de navegación para la relación uno a muchos con MovimientoInventario
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}
