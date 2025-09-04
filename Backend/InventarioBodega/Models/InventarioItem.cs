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

        [ForeignKey("Inventario")]
        public int IdInventario { get; set; } // Clave foránea a IdInvenatrio

        
        public Inventario Inventario { get; set; } // Propiedad de navegación

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PesoActual { get; set; } = 0;

        [Required]
        public DateTime FechaRegistroItem { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } // Ej: "REGISTRADO", "EN_ALMACEN", "DESPACHADO"
        // Propiedad de navegación para la relación uno a muchos con MovimientoInventario
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}
