namespace Inventario.DTOS
{
    public class ProductosDTO
    {
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public bool Estado { get; set; }

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }

        public int IdMarca { get; set; }
        public string NombreMarca { get; set; }

        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }

        public int IdUnidad { get; set; }
        public string NombreUnidad { get; set; }

        public DateOnly? FechaExpiracion { get; set; }
    }
}
