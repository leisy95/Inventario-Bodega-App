namespace InventarioBackend.Models
{
    public class Salida
    {
        public int Id { get; set; }

        public string Usuario { get; set; } = string.Empty;

        // EN_PROCESO | CONFIRMADA | CANCELADA
        public string Estado { get; set; } = "EN_PROCESO";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaCancelacion { get; set; }

        // Relación con Cliente
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // Relación con items
        public ICollection<SalidaItem> Items { get; set; } = new List<SalidaItem>();
    }
}
