using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.DTOs
{
    public class RegistrarItemRequest
    {
        [Required(ErrorMessage = "La Referencia (Tipo) es obligatoria.")]
        public string Referencia { get; set; }
        public string Descripcion { get; set; }

        public string? TipoBolsa { get; set; }
        public string? TipoMaterial { get; set; }
        public string ImpresoNo { get; set; }
        public decimal? Ancho { get; set; }

        public decimal? Alto { get; set; }
        public decimal? Calibre { get; set; }
        public string Color { get; set; }
        public string? SegundoColor { get; set; }
        public string? Densidad { get; set; }
    }

    public class RegistrarItemResponse
    {
        public int IdItemGenerado { get; set; }
        public string Referencia { get; set; }
        public string Message { get; set; }
    }
}
