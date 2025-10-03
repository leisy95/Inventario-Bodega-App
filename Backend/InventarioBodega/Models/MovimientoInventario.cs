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
        public string Referencia { get; set; } 

        [Required]
        [MaxLength(60)]
        public string ReferenciaPeso { get; set; } 

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Peso { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; } 

        [MaxLength(100)]
        public string Usuario { get; set; }

        [ForeignKey("Inventario")]
        public int IdInventario { get; set; }
        public Inventario Inventario { get; set; }

        [ForeignKey("InventarioItem")]
        public int IdInventarioItem { get; set; }
        public InventarioItem InventarioItem { get; set; }

        // Relación con Cliente (opcional, solo en salidas)
        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}