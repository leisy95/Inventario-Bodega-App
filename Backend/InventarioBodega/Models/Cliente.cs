using System.ComponentModel.DataAnnotations;

namespace InventarioBackend.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [MaxLength(200)]
        public string Direccion { get; set; }

        [MaxLength(50)]
        public string Telefono { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        // Relación con salidas
        public ICollection<Salida> Salidas { get; set; }
    }
}
