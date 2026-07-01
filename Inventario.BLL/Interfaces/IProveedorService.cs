using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface IProveedorService
    {
        Task<List<ProveedoresDTO>> Lista();
        Task<ProveedoresDTO> Crear(ProveedoresDTO entidad);
        Task<bool> Editar(ProveedoresDTO entidad);
        Task<bool> Eliminar(int idProveedor);
        Task<Proveedores> ObtenerPorId(int idProveedor);
    }
}
