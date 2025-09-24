namespace InventarioBackend.DTOs
{
    public class MovimientoInventarioRequest
    {
        public string Referencia { get; set; }
        public string ReferenciaPeso { get; set; }
        public decimal Peso { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Usuario { get; set; }
        public int IdInventario { get; set; }
        public int IdInventarioItem { get; set; }
    }

    public class MovimientoInventarioResponse
    {
        public int Id { get; set; }
        public string Referencia { get; set; }
        public string ReferenciaPeso { get; set; }
        public decimal Peso { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Usuario { get; set; }
        public string InventarioNombre { get; set; }
        public string InventarioItemCodigo { get; set; }
    }
}
