using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface IUnidadesService
    {
        Task<List<UnidadMedidaDTO>> Lista();
        Task<UnidadMedidaDTO> ObtenerPorId(int idMedida);
        Task<UnidadMedidaDTO> Crear(UnidadMedidaDTO entidad);
        Task<bool> Editar(UnidadMedidaDTO entidad);
        Task<bool> Eliminar(int idMedida);
    }
}

