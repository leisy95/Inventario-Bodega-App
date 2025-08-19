
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioBackend.Models
{
    [Table("Inventario")]
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Referencia { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Column("Tipo de Bolsa")]
        [StringLength(100)]
        public string TipoBolsa { get; set; }

        [Column("Tipo Material")]
        [StringLength(100)]
        public string TipoMaterial { get; set; }

        [StringLength(50)]
        public string Densidad { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        [Column("Segundo color")]
        [StringLength(50)]
        public string SegundoColor { get; set; }

        [Column("Impreso/No")]
        [StringLength(20)]
        public string ImpresoNo { get; set; }

        public decimal Ancho { get; set; }
        public decimal Alto { get; set; }
        public decimal Calibre { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Peso { get; set; } = 0; 

        public int Cantidad { get; set; } = 0; 
    }
}