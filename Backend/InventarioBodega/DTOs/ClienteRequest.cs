using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.DTOs
{
    public class ClienteRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres")]
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}
