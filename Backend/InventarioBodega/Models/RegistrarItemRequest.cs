using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.Models
{
    // Class for the request to register a new item
    public class RegistrarItemRequest
    {
        [Required]
        public string ReferenciaTipo { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Medida { get; set; }
        [Required]
        public string UnidadPeso { get; set; } = string.Empty;

        public decimal? Peso { get; set; }

        // New fields for the ticket
        public string? Ancho { get; set; }
        public string? Largo { get; set; }
        public string? Calibre { get; set; }
        public string? Material { get; set; }
        public string? Mezcla { get; set; }
        public string? Toquel { get; set; }
    }

    // Class for the response after registering an item
    public class RegistrarItemResponse
    {
        public int IdItemGenerado { get; set; }
        public string ReferenciaTipo { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

