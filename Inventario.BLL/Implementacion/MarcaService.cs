using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;


namespace Inventario.BLL.Implementacion
{
    public class MarcaService : IMarcaService
    {
        private readonly IGenericRepository<Marcas> _repositorio;
        private readonly IMapper _mapper;

        public MarcaService(IGenericRepository<Marcas> repositorio,IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<List<MarcasDTO>> Lista()
        {
            IQueryable<Marcas> query = await _repositorio.Consultar();
            var ListaEntidades = query.ToList();

            if (ListaEntidades == null) 
            {
                throw new TaskCanceledException("No se encontraron Maracas registrados en el sistema.");
            }

            return _mapper.Map<List<MarcasDTO>>(ListaEntidades);
        }

        public async Task<MarcasDTO> Crear(MarcasDTO marcaDTO)
        {
            try
            {
                Marcas existeNombre = await _repositorio.ObtenerDatos(m => m.NombreMarca.ToLower() == marcaDTO.NombreMarca.ToLower().Trim());

                if (existeNombre != null)
                {
                    throw new Exception("Ya existe una marca con ese nombre");

                }               

                Marcas entidad = _mapper.Map<Marcas>(marcaDTO);
                Marcas creado = await _repositorio.Crear(entidad);

                if (creado.IdMarca == 0)
                    throw new Exception("No se pudo crear la marca.");

                return _mapper.Map<MarcasDTO>(creado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear marca: " + ex.Message);
            }
        }

        public async Task<bool> Editar(MarcasDTO marcaDTO)
        {
            try
            {
                Marcas existente = await _repositorio.ObtenerDatos(m => m.IdMarca == marcaDTO.IdMarca);
                
                if (existente == null)
                {
                    throw new TaskCanceledException("Marca no encontrada.");
                }

                IQueryable<Marcas> duplicado = await _repositorio.Consultar(
                   m => m.NombreMarca.ToLower() == marcaDTO.NombreMarca.ToLower()
                     && m.IdMarca != marcaDTO.IdMarca
               );

                if (duplicado.Any())
                {
                    throw new TaskCanceledException("Ya existe otra medida con ese nombre.");
                }

                existente.NombreMarca = marcaDTO.NombreMarca;
                existente.Descripcion = marcaDTO.Descripcion;

                bool respuesta = await _repositorio.Editar(existente);
                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo actualizar la marca en la base de datos.");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar marca: " + ex.Message);
            }
        }

        public async Task<bool> Eliminar(int idMarca)
        {
            try
            {
                Marcas marca = await _repositorio.ObtenerDatos(m => m.IdMarca == idMarca);

                if (marca == null)
                {
                    throw new TaskCanceledException("La marca no existe.");
                }

                return await _repositorio.Eliminar(marca);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar marca: " + ex.Message);
            }
        }

        public async Task<MarcasDTO> ObtenerPorId(int idMarca)
        {
            var entidad = await _repositorio.ObtenerDatos(m => m.IdMarca == idMarca);

            if (entidad == null)
            {
                throw new TaskCanceledException($"El rol con ID {idMarca} no existe.");
            }

            return _mapper.Map<MarcasDTO>(entidad);

        }
    }
}
