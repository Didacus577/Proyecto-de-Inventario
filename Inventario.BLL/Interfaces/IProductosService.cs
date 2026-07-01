using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface IProductosService
    {

        Task<List<ProductosDTO>> Lista();
        Task<ProductosDTO> Crear(ProductosDTO entidad);
        Task<bool> Editar(ProductosDTO entidad);
        Task<bool> Eliminar(int idProducto);
        Task<ProductosDTO> ObtenerPorId(int idProducto);
    }
}
