using Inventario.DTOS;


namespace Inventario.BLL.Interfaces
{
    public interface ILoginService
    {
        Task<UsuarioDTO> Login(UsuarioDTO entidad);
    }
}
