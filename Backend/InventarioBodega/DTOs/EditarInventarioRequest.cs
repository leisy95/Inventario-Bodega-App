using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.DTOs
{
    public class EditarInventarioRequest
    {
        public string Referencia { get; set; }

        public string Descripcion { get; set; }

        public string TipoBolsa { get; set; }
        public string TipoMaterial { get; set; }
        public string Densidad { get; set; }
        public string Color { get; set; }
        public string SegundoColor { get; set; }
        public string ImpresoNo { get; set; }

        [Required(ErrorMessage = "El ancho es obligatorio.")]
        public decimal Ancho { get; set; }

        [Required(ErrorMessage = "El alto es obligatorio.")]
        public decimal Alto { get; set; }

        [Required(ErrorMessage = "El calibre es obligatorio.")]
        public decimal Calibre { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio.")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Cantidad { get; set; }
    }
}