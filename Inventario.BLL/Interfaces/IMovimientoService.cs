using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface IMovimientoService
    {
        Task<List<MovimientoInventarioDTO>> Lista();
        Task<MovimientoInventarioDTO> Crear(MovimientoInventarioDTO entidad);
        Task<bool> Editar(MovimientoInventarioDTO entidad);
        Task<bool> Eliminar(int idMovimiento);
        Task<MovimientoInventarioDTO> ObtenerPorId(int idMovimiento);
        
    }
}
