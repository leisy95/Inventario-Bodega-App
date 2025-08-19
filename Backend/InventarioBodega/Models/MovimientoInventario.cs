using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioBackend.Models
{
    public class MovimientoInventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMovimiento { get; set; }

        [Required]
        public int IdItem { get; set; } // Clave foránea a InventarioItem

        [ForeignKey("IdItem")]
        public InventarioItem InventarioItem { get; set; } // Propiedad de navegación

        [Required]
        public DateTime FechaMovimiento { get; set; }

        [Required]
        [StringLength(20)]
        public string TipoMovimiento { get; set; } // Ej: "ENTRADA", "SALIDA"

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CantidadPeso { get; set; }

        [StringLength(255)]
        public string? Observaciones { get; set; } // Anulable
    }
}
