using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Inventario.BLL.Implementacion
{
    public class ProductoService : IProductosService
    {

        private readonly IGenericRepository<Productos> _repositorio;
        private readonly IProductosRepository _productoRepository;
        private readonly IMapper _mapper;


        public ProductoService(IGenericRepository<Productos> repositorio, IProductosRepository productoRepository,IMapper mapper)
        {
            _repositorio = repositorio;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductosDTO>> Lista()
        {
            try
            {
                IQueryable<Productos> query = await _repositorio.Consultar();
                var lista = await query
                    .Include(p => p.IdCategoriaNavigation)
                    .Include(p => p.IdMarcaNavigation)
                    .Include(p => p.IdProveedorNavigation)
                    .Include(p => p.IdUnidadNavigation)
                    .ToListAsync();

                return _mapper.Map<List<ProductosDTO>>(lista);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de productos: " + ex.Message);
            }
        }

        public async Task<ProductosDTO> Crear(ProductosDTO productoDTO)
        {

            Productos existeNombre = await _repositorio.ObtenerDatos(p => p.NombreProducto.ToLower() == productoDTO.NombreProducto.ToLower().Trim());

            if (existeNombre != null) 
            {
                throw new Exception("Ya existe un producto con ese nombre");

            }
            const string nombreSP = "sp_InsertProducto";

            
            Productos entidad = _mapper.Map<Productos>(productoDTO);

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@NombreProducto", entidad.NombreProducto),
                new SqlParameter("@Descripcion", entidad.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@Stock", entidad.Stock),
                new SqlParameter("@StockMinimo", entidad.StockMinimo),
                new SqlParameter("@Estado", entidad.Estado),
                new SqlParameter("@IdCategoria", entidad.IdCategoria),
                new SqlParameter("@IdMarca", entidad.IdMarca),
                new SqlParameter("@IdProveedor", entidad.IdProveedor),
                new SqlParameter("@IdUnidad", entidad.IdUnidad),
                new SqlParameter("@FechaExpiracion", entidad.FechaExpiracion.HasValue
                    ? (object)new DateTime(entidad.FechaExpiracion.Value.Year, entidad.FechaExpiracion.Value.Month, entidad.FechaExpiracion.Value.Day)
                    : (object)DBNull.Value) { SqlDbType = SqlDbType.Date }
            };

            try
            {
                int filasAfectadas = await _productoRepository.EjecutarSP(nombreSP, parametros);

                if (filasAfectadas > 0)                {
                    
                    return productoDTO;
                }
                else
                {
                    throw new Exception("El procedimiento almacenado no insertó el producto.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fallo al crear el producto. Detalle: {ex.Message}");
            }
        }

        public async Task<bool> Editar(ProductosDTO productosDTO)
        {
            try
            {
                Productos existente = await _repositorio.ObtenerDatos(p => p.IdProducto == productosDTO.IdProducto);                

                if (existente == null)
                    throw new TaskCanceledException("El producto no existe.");
               
                existente.NombreProducto = productosDTO.NombreProducto;
                existente.Descripcion = productosDTO.Descripcion;
                existente.Stock = productosDTO.Stock;
                existente.StockMinimo = productosDTO.StockMinimo;
                existente.Estado = productosDTO.Estado;
                existente.IdCategoria = productosDTO.IdCategoria;
                existente.IdMarca = productosDTO.IdMarca;
                existente.IdProveedor = productosDTO.IdProveedor;
                existente.IdUnidad = productosDTO.IdUnidad;
                existente.FechaExpiracion = productosDTO.FechaExpiracion;

                bool respuesta = await _repositorio.Editar(existente);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el producto: " + ex.Message);
            }
        }

        public  async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                Productos producto = await _repositorio.ObtenerDatos(p => p.IdProducto == idProducto);

                if (producto == null)
                    throw new TaskCanceledException("El producto no existe.");

                return await _repositorio.Eliminar(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar producto: " + ex.Message);
            }
        }


        public async Task<ProductosDTO> ObtenerPorId(int idProducto)
        {
            IQueryable<Productos> query = await _repositorio.Consultar(p => p.IdProducto == idProducto);

            Productos producto = await query
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdMarcaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .Include(p => p.IdUnidadNavigation)
                .FirstOrDefaultAsync();

            if (producto == null)
                throw new TaskCanceledException("El producto no existe.");

            return _mapper.Map<ProductosDTO>(producto);
        }
    }
}
