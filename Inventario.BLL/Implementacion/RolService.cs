using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;

namespace Inventario.BLL.Implementacion
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Roles> _repositorio;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Roles> repositorio,IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

       
        public async Task<List<RolDTO>> Lista()
        {
            try
            {
                IQueryable<Roles> query = await _repositorio.Consultar();
                var listaEntidades = query.ToList();

                if (listaEntidades == null)
                {
                    throw new TaskCanceledException("No se encontraron roles registrados en el sistema.");
                }

                return _mapper.Map<List<RolDTO>>(listaEntidades);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
           
        }

       
        public async Task<RolDTO> ObtenerPorId(int idRol)
        {
            try
            {
                var entidad =  await _repositorio.ObtenerDatos(r => r.IdRol == idRol);

                if (entidad == null)
                {
                    throw new TaskCanceledException($"El rol con ID {idRol} no existe.");
                }

                return _mapper.Map<RolDTO>(entidad);

            }           
            catch (Exception ex)
            {
                
                throw new Exception("Se produjo un error al buscar el rol solicitado.", ex);
            }

        }

        
        public async Task<RolDTO> Crear(RolDTO rolDTO)
        {
            try
            {
               
               
                var query = await _repositorio.Consultar(
                    r => r.NombreRol.ToLower() == rolDTO.NombreRol.ToLower()
                );

                if (query.Any())
                {
                    throw new TaskCanceledException("El rol ya existe en el sistema.");
                }

                Roles entidad = _mapper.Map<Roles>(rolDTO);
                Roles NuevoRol = await _repositorio.Crear(entidad);

                if (NuevoRol.IdRol == 0)
                    throw new TaskCanceledException("No se pudo crear el rol.");

                return _mapper.Map<RolDTO>(NuevoRol); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el rol: " + ex.Message);
            }
        }

       
        public async Task<bool> Editar(RolDTO rolDTO)
        {
            try
            {
                Roles rolExistente = await _repositorio.ObtenerDatos(r => r.IdRol == rolDTO.IdRol);

                if (rolExistente == null)
                {
                    throw new TaskCanceledException("El rol no existe.");
                }                 
                                
                IQueryable<Roles> duplicado = await _repositorio.Consultar(
                    r => r.NombreRol.ToLower() == rolDTO.NombreRol.ToLower()
                      && r.IdRol != rolDTO.IdRol
                );

                if (duplicado.Any())
                {
                    throw new TaskCanceledException("Ya existe otro rol con ese nombre.");
                }

                rolExistente.NombreRol = rolDTO.NombreRol;

                bool respuesta = await _repositorio.Editar(rolExistente);

                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo actualizar el rol en la base de datos.");
                }

                return respuesta;              

               
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el rol: " + ex.Message);
            }
        }

       
        public async Task<bool> Eliminar(int idRol)
        {
            try
            {
                Roles rol = await _repositorio.ObtenerDatos(r => r.IdRol == idRol);

                if (rol == null)
                {
                    throw new TaskCanceledException("El rol no existe.");
                }
                  

                return await _repositorio.Eliminar(rol);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el rol: " + ex.Message);
            }
        }
    }
}
