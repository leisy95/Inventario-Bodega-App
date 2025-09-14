using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioBackend.Models
{
    public class MovimientoInventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Referencia { get; set; } // Ej: 1R0BL003004

        [Required]
        [MaxLength(60)]
        public string ReferenciaPeso { get; set; } // Ej: 1R0BL0030044

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Peso { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; } // "Entrada" | "Salida"

        [MaxLength(100)]
        public string Usuario { get; set; }

        // Relación con Inventario (general)
        [ForeignKey("Inventario")]
        public int IdInventario { get; set; }
        public Inventario Inventario { get; set; }

        // Relación con InventarioItem (paquete exacto)
        [ForeignKey("InventarioItem")]
        public int IdInventarioItem { get; set; }
        public InventarioItem InventarioItem { get; set; }
    }
}