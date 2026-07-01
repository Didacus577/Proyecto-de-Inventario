using Inventario.DTOS; 
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.Entity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Inventario.BLL.Implementacion
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IGenericRepository<Usuarios> _repositorio;
        private readonly IMapper _mapper;

        public UsuariosService(IGenericRepository<Usuarios> repositorio,IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

       
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                IQueryable<Usuarios> query = await _repositorio.Consultar();
                List<Usuarios> listaEntidades = query.Include(r => r.IdRolNavigation).ToList();

                if (listaEntidades == null)
                {
                    throw new TaskCanceledException("No se encontraron usuarios registrados en el sistema.");
                }

                return _mapper.Map<List<UsuarioDTO>>(listaEntidades);

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            
        }

        
        public async Task<UsuarioDTO> Crear(UsuarioDTO usuarioDTO)
        {
           
            Usuarios usuarioExiste = await _repositorio.ObtenerDatos(u => u.Correo == usuarioDTO.Correo);
            if (usuarioExiste != null)
            {
                throw new TaskCanceledException("El correo ya está registrado.");
            }               

            try
            {
                Usuarios Entidad = _mapper.Map<Usuarios>(usuarioDTO);

                Usuarios usuarioCreado = await _repositorio.Crear(Entidad);

                if (usuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario.");
                }

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear usuario: " + ex.Message);
            }
        }

       
        public async Task<bool> Editar(UsuarioDTO usuarioDTO)
        {
            // Validar que no haya otro usuario con el mismo correo
            Usuarios usuarioExiste = await _repositorio.ObtenerDatos(u => u.Correo == usuarioDTO.Correo && u.IdUsuario != usuarioDTO.IdUsuario);
            if (usuarioExiste != null)
            {
                throw new TaskCanceledException("El correo ya está en uso por otro usuario.");
            }

            try
            {
                Usuarios usuarioEditar = await _repositorio.ObtenerDatos(u => u.IdUsuario == usuarioDTO.IdUsuario);

                if (usuarioEditar == null)
                {
                    throw new TaskCanceledException("El usuario no existe.");
                }

               
                usuarioEditar.NombreUsuario = usuarioDTO.NombreUsuario;
                usuarioEditar.Correo = usuarioDTO.Correo;
                usuarioEditar.IdRol = usuarioDTO.IdRol;

                
                if (!string.IsNullOrWhiteSpace(usuarioDTO.Clave))
                {
                    usuarioEditar.Clave = usuarioDTO.Clave;
                }

                bool respuesta = await _repositorio.Editar(usuarioEditar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el usuario.");

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                Usuarios usuario = await _repositorio.ObtenerDatos(u => u.IdUsuario == idUsuario);

                if (usuario == null)
                    throw new TaskCanceledException("El usuario no existe.");

                bool respuesta = await _repositorio.Eliminar(usuario);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario: " + ex.Message);
            }
        }

        public async Task<UsuarioDTO> ObtenerPorId(int idUsuario)
        {
            try
            {
                IQueryable<Usuarios> query = await _repositorio.Consultar(u => u.IdUsuario == idUsuario);
                Usuarios usuario = query.Include(r => r.IdRolNavigation).FirstOrDefault();

                if (usuario == null)
                {
                    throw new TaskCanceledException("El usuario no existe.");
                }                  

                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el usuario: " + ex.Message);
            }
        }
    }
}
