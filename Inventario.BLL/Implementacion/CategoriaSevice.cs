using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.Entity;
using Inventario.DTOS;
using AutoMapper;

namespace Inventario.BLL.Implementacion
{
    public class CategoriaSevice : ICategoriaService
    {

        private readonly IGenericRepository<Categoria> _repositorio;
        private readonly IMapper _mapper;


        public CategoriaSevice(IGenericRepository<Categoria> repositorio, IMapper mapper) 
        { 
            _repositorio = repositorio;
            _mapper = mapper;

        }


        public async Task<List<CategoriaDTO>> Lista() 
        {
            try
            {
                IQueryable<Categoria> query = await _repositorio.Consultar();
                var listaEntidades = query.ToList();

                if (listaEntidades == null)
                {
                    throw new TaskCanceledException("No se encontraron Categorias registrados en el sistema.");
                }

                return _mapper.Map<List<CategoriaDTO>>(listaEntidades);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<CategoriaDTO> Crear(CategoriaDTO categoriaDTO) 
        {
            try 
            {
               Categoria existeNombre = await _repositorio.ObtenerDatos(c => c.NombreCategoria.ToLower() == categoriaDTO.NombreCategoria.ToLower().Trim());

                if (existeNombre != null)
                {
                    throw new Exception("Ya existe una categoria con ese nombre");

                }

                Categoria entidad = _mapper.Map<Categoria>(categoriaDTO);
                Categoria NuevaCategoria = await _repositorio.Crear(entidad);

                if(NuevaCategoria.IdCategoria == 0)
                {
                    throw new TaskCanceledException("No se pudo registrar la categoria.");
                }

                return _mapper.Map<CategoriaDTO>(NuevaCategoria);

            }catch(Exception ex)           
            {
                throw new Exception("Error al crear la categoria: " + ex.Message);
            }         


        }


        public async Task<bool> Editar(CategoriaDTO categoriaDTO) 
        {
            try
            {

                Categoria CategoriaExistente = await _repositorio.ObtenerDatos(c => c.IdCategoria == categoriaDTO.IdCategoria);
                                

                if (CategoriaExistente == null)
                {
                    throw new TaskCanceledException("La categoria no existe.");
                }

                IQueryable<Categoria> duplicado = await _repositorio.Consultar(c => 
                c.NombreCategoria.ToLower() == categoriaDTO.NombreCategoria.ToLower()
                 && c.IdCategoria != categoriaDTO.IdCategoria                 
                 );

                if (duplicado.Any())
                {
                    throw new TaskCanceledException("Ya existe otra categoria con ese nombre.");
                }

                CategoriaExistente.IdCategoria = categoriaDTO.IdCategoria;
                CategoriaExistente.NombreCategoria = categoriaDTO.NombreCategoria;
                CategoriaExistente.Descripcion = categoriaDTO.Descripcion;

                bool respuesta = await _repositorio.Editar(CategoriaExistente);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar la categoria.");
                }

                return respuesta;

            }
            catch (Exception ex) 
            {
                throw new Exception("Error al editar la categoria: " + ex.Message);
            }


        }

        public async Task<bool> Eliminar(int idCategoria) 
        {
            try
            {
                Categoria categoriaEncontrada = await _repositorio.ObtenerDatos(c => c.IdCategoria == idCategoria);
                if (categoriaEncontrada == null)
                {
                    throw new TaskCanceledException("La categoria no existe.");
                }

                bool respuesta = await _repositorio.Eliminar(categoriaEncontrada);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar la categoria.");
                }

                return respuesta;

            } catch (Exception ex)
            {
                throw new Exception("Error al eliminar la categoria: " + ex.Message);
            } 
           
        
        }
       

        public async Task<CategoriaDTO> ObtenerPorId(int idcategoria)
        {
            try
            {
                Categoria entidad = await _repositorio.ObtenerDatos(c => c.IdCategoria == idcategoria);


                if (entidad == null)
                {
                    throw new TaskCanceledException("La categoria no existe.");
                }

                return _mapper.Map<CategoriaDTO>(entidad);

            }            
            catch (Exception ex)
            {
               
                throw new Exception("Se produjo un error al buscar la categoria solicitada.", ex);
            }
           
         
        }


        
    }
}
