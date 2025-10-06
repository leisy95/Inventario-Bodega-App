namespace InventarioBackend.DTOs
{
    public class EditarClienteRequest
    {
        public int Id { get; set; }  // Id del cliente a actualizar
        public string Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}
