using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;




namespace Inventario.BLL.Servicios
{
    public class UnidadesMedidaService : IUnidadesService
    {
        private readonly IGenericRepository<UnidadesMedida> _repositorio;
        private readonly IMapper _mapper;

        public UnidadesMedidaService(IGenericRepository<UnidadesMedida> repositorio,IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<List<UnidadMedidaDTO>> Lista()
        {
            try
            {
                IQueryable<UnidadesMedida> query = await _repositorio.Consultar();
                var listaEntidades = query.ToList();

                if (listaEntidades == null)
                {
                    throw new TaskCanceledException("No se encontraron roles registrados en el sistema.");
                }

                return _mapper.Map<List<UnidadMedidaDTO>>(listaEntidades);

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<UnidadMedidaDTO> ObtenerPorId(int idMedida)
        {
            try
            {
                var entidad = await _repositorio.ObtenerDatos(u => u.IdUnidad == idMedida);

                if (entidad == null)
                {
                    throw new TaskCanceledException("La unidad de medida no existe.");
                }

                return _mapper.Map<UnidadMedidaDTO>(entidad);

            }
            catch (Exception ex) 
            {
                throw new Exception("Se produjo un error al bucar la unidad de midad solicitada: " + ex.Message);
            }        
                       
        }

        public async Task<UnidadMedidaDTO> Crear(UnidadMedidaDTO medidaDTO)
        {
            try
            {

                UnidadesMedida existeNombre = await _repositorio.ObtenerDatos(u => u.NombreUnidad.ToLower() == medidaDTO.NombreUnidad.ToLower().Trim());

                if (existeNombre != null)
                {
                    throw new Exception("Ya existe un producto con ese nombre");

                }

                UnidadesMedida entidad = _mapper.Map<UnidadesMedida>(medidaDTO);
                UnidadesMedida NuevaMedida = await _repositorio.Crear(entidad);

                if (NuevaMedida.IdUnidad == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la unidad de medida.");
                }

                return _mapper.Map<UnidadMedidaDTO>(NuevaMedida);
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al crear la unidad de medida: " + ex.Message);
            }     
                      
        }

        public async Task<bool> Editar(UnidadMedidaDTO medidaDTO)
        {
            try
            {
                UnidadesMedida unidadExistente = await _repositorio.ObtenerDatos(u => u.IdUnidad == medidaDTO.IdUnidad);

                if (unidadExistente == null)
                {
                    throw new TaskCanceledException("La unidad de medida no existe.");
                }

                IQueryable<UnidadesMedida> duplicado = await _repositorio.Consultar(
                       m => m.NombreUnidad.ToLower() == medidaDTO.NombreUnidad.ToLower()
                         && m.IdUnidad != medidaDTO.IdUnidad
                   );

                if (duplicado.Any())
                {
                    throw new TaskCanceledException("Ya existe otra unidad de medida con ese nombre.");
                }

                unidadExistente.NombreUnidad = medidaDTO.NombreUnidad;
                unidadExistente.Abreviatura = medidaDTO.Abreviatura;

                bool respuesta = await _repositorio.Editar(unidadExistente);

                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo actualizar la unidad de medida en la base de datos.");
                }

                return respuesta;

            }
            catch (Exception ex) 
            {
                throw new Exception("Error al editar la unidad de medida: " + ex.Message);
            }       
        }

        public async Task<bool> Eliminar(int idMedida)
        {
            try
            {   
                 UnidadesMedida unidadMedida = await _repositorio.ObtenerDatos(m => m.IdUnidad == idMedida);

                if (unidadMedida == null)
                {
                    throw new TaskCanceledException("La unidad de medida no se encuentra");
                }

                 return await _repositorio.Eliminar(unidadMedida);

            }
            catch (Exception ex) 
            {
                throw new Exception("Error al eliminar la unidad de medida: " + ex.Message);
            }
        }
    }
}