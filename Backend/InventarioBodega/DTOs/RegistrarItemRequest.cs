using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.DTOs
{
    public class RegistrarItemRequest
    {
        [Required(ErrorMessage = "La Referencia (Tipo) es obligatoria.")]
        public string ReferenciaTipo { get; set; }

        public string? Color { get; set; }
        public string? Medida { get; set; }

        [Required(ErrorMessage = "La Unidad de Peso es obligatoria.")]
        public string UnidadPeso { get; set; }

        public decimal? Peso { get; set; }

        public decimal? Ancho { get; set; }
        public decimal? Largo { get; set; }
        public decimal? Calibre { get; set; }
        public string? Material { get; set; }
        public string? Mezcla { get; set; }
        public string? Toquel { get; set; }
    }

    public class RegistrarItemResponse
    {
        public int IdItemGenerado { get; set; }
        public string ReferenciaTipo { get; set; }
        public string Message { get; set; }
    }
}
