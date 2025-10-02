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
        public int IdInventario { get; set; } 

        
        public Inventario Inventario { get; set; } 
        [StringLength(200)]
        public string ReferenciaPeso { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PesoActual { get; set; } = 0;

        [Required]
        public DateTime FechaRegistroItem { get; set; }       

        [StringLength(50)]
        public string Estado { get; set; } 
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    }
}
