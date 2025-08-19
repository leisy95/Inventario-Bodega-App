using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.DTOs
{
    public class IngresarPesoRequest
    {
        [Required(ErrorMessage = "La referencia es obligatoria.")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio.")]
        public decimal Peso { get; set; }
    }

    public class IngresarPesoResponse
    {
        public string Referencia { get; set; }
        public decimal Peso { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Ancho { get; set; }
        public decimal Alto { get; set; }
        public decimal Calibre { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
    }
}
