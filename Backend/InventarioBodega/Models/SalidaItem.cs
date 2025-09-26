namespace InventarioBackend.Models
{
    public class SalidaItem
    {
        public int Id { get; set; }

        public int IdSalida { get; set; }
        public Salida Salida { get; set; } = null!;

        public int IdInventarioItem { get; set; }
        public InventarioItem InventarioItem { get; set; } = null!;

        public DateTime FechaAgregado { get; set; } = DateTime.Now;
    }
}
