using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Interfaces
{
    public interface IUsuariosService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<UsuarioDTO> Crear(UsuarioDTO entidad);
        Task<bool> Editar(UsuarioDTO entidad);
        Task<bool> Eliminar(int idUsuario);
        Task<UsuarioDTO> ObtenerPorId(int IdUsuario);
    }
}
