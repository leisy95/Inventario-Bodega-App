using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InventarioBackend.Models
{
    public class TipoProducto
    {
        [Key] // Marca IdTipo como la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indica que la base de datos generará el valor
        public int IdTipo { get; set; }

        [Required]
        [StringLength(100)] // Aumentado a 100 para más flexibilidad en la referencia
        public string Referencia { get; set; }

        [StringLength(50)]
        public string? Color { get; set; } // Anulable

        [StringLength(50)]
        public string? Medida { get; set; } // Anulable

        [StringLength(20)]
        public string? UnidadPeso { get; set; } // Anulable

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Peso { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Ancho { get; set; } // Anulable en la DB y en el modelo

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Largo { get; set; } // Anulable en la DB y en el modelo

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Calibre { get; set; } // Anulable en la DB y en el modelo

        [StringLength(100)]
        public string? Material { get; set; } // Anulable

        [StringLength(100)]
        public string? Mezcla { get; set; } // Anulable

        [StringLength(50)]
        public string? Toquel { get; set; } // Anulable

        // Propiedad de navegación para la relación uno a muchos con InventarioItem
        public ICollection<InventarioItem> InventarioItems { get; set; } = new List<InventarioItem>();
    }
}
