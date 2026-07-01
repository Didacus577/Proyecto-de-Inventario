using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Implementacion
{
    public class LoginService : ILoginService
    {
        private readonly IGenericRepository<Usuarios> _repositorio;
        private readonly IGenericRepository<Roles> _rolRepositorio; 
        private readonly IUtilidadesService _utilidadesService;
        private readonly IMapper _mapper;
    
        public LoginService(
            IGenericRepository<Usuarios> repositorio,
            IGenericRepository<Roles> rolRepositorio,
            IUtilidadesService utilidadesService,
            IMapper mapper)
        {
            _repositorio = repositorio;
            _rolRepositorio = rolRepositorio;
            _utilidadesService = utilidadesService;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> Login(UsuarioDTO usuarioDTO)
        {
            try
            {
                string claveEncriptada = _utilidadesService.ConvertirSHA256(usuarioDTO.Clave);

                Usuarios usuario = await _repositorio.ObtenerDatos(
                    u => u.Correo == usuarioDTO.Correo && u.Clave == claveEncriptada
                );

                if (usuario == null)
                {
                    throw new TaskCanceledException("Credenciales incorrectas");
                }
                
                var rolEntidad = await _rolRepositorio.ObtenerDatos(r => r.IdRol == usuario.IdRol);

              
                var resultadoDTO = _mapper.Map<UsuarioDTO>(usuario);

                if (rolEntidad != null)
                {
                    resultadoDTO.NombreRol = rolEntidad.NombreRol?.ToLower().Trim();
                }

                return resultadoDTO;
            }
            catch
            {
                throw;
            }
        }
    }
}