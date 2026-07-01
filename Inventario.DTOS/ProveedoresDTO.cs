namespace Inventario.DTOS
{
    public class ProveedoresDTO
    {
        public int IdProveedor { get; set; }

        public string NombreProveedor { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public string? Direccion { get; set; }

    }
}
