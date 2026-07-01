namespace Inventario.DTOS
{
    public class MovimientoInventarioDTO
    {
        public int IdMovimiento { get; set; }

        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }

        public int IdUsuario { get; set; }       

        public string TipoMovimiento { get; set; } = null!;

        public int Cantidad { get; set; }

        public DateTime FechaMovimiento { get; set; }
        public object NombreUsuario { get; set; }
    }
}
