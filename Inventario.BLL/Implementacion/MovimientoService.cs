using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;
using Microsoft.EntityFrameworkCore;

namespace Inventario.BLL.Implementacion
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IGenericRepository<MovimientosInventario> _repositorio;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Productos> _productoRepository;

        public MovimientoService(IGenericRepository<MovimientosInventario> repositorio, IMapper mapper, IGenericRepository<Productos> productoRepository)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _productoRepository = productoRepository;
        }

        public async Task<List<MovimientoInventarioDTO>> Lista()
        {
            try
            {
                IQueryable<MovimientosInventario> query = await _repositorio.Consultar();

                var listaEntidad = await query
                    .Include(m => m.IdProductoNavigation)
                    .Include(m => m.IdUsuarioNavigation)
                    .OrderByDescending(m => m.FechaMovimiento)
                    .ToListAsync();

                return _mapper.Map<List<MovimientoInventarioDTO>>(listaEntidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar movimientos: " + ex.Message);
            }
        }

        public async Task<MovimientoInventarioDTO> Crear(MovimientoInventarioDTO movimientoDTO)
        {
            try
            {
                // 1. Validar y actualizar el stock de la tabla Productos usando tus nombres reales
                await ActualizarStockProducto(movimientoDTO.IdProducto, movimientoDTO.TipoMovimiento, movimientoDTO.Cantidad);

               
                MovimientosInventario entidad = _mapper.Map<MovimientosInventario>(movimientoDTO);
                entidad.FechaMovimiento = DateTime.Now;

                // 3. Registrar el movimiento en la base de datos
                MovimientosInventario creado = await _repositorio.Crear(entidad);

                if (creado.IdMovimiento == 0)
                {
                    throw new TaskCanceledException("No se pudo registrar el movimiento de inventario.");
                }

                return _mapper.Map<MovimientoInventarioDTO>(creado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el movimiento: " + ex.Message);
            }
        }

        public async Task<bool> Editar(MovimientoInventarioDTO movimientoDTO)
        {
            try
            {
              
                MovimientosInventario existente = await _repositorio.ObtenerDatos(m => m.IdMovimiento == movimientoDTO.IdMovimiento);

                if (existente == null)
                {
                    throw new TaskCanceledException("El movimiento no existe.");
                }

                
                // Si el viejo fue Entrada, restamos. Si fue Salida, sumamos
                string tipoInverso = existente.TipoMovimiento.Equals("Entrada", StringComparison.OrdinalIgnoreCase) ? "Salida" : "Entrada";
                await ActualizarStockProducto(existente.IdProducto, tipoInverso, existente.Cantidad);

                // 3. APLICAR EL STOCK DEL NUEVO MOVIMIENTO EDITADO
                // Esto valida si hay stock suficiente en caso de que sea una nueva salida
                await ActualizarStockProducto(movimientoDTO.IdProducto, movimientoDTO.TipoMovimiento, movimientoDTO.Cantidad);

                // 4. Asignar los nuevos datos al registro existente
                existente.IdProducto = movimientoDTO.IdProducto;
                existente.IdUsuario = movimientoDTO.IdUsuario;
                existente.TipoMovimiento = movimientoDTO.TipoMovimiento;
                existente.Cantidad = movimientoDTO.Cantidad;

                // 5. Guardar los cambios del movimiento en la base de datos
                bool respuesta = await _repositorio.Editar(existente);
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el movimiento.");

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar movimiento: " + ex.Message);
            }
        }

        public async Task<bool> Eliminar(int idMovimiento)
        {
            try
            {
                MovimientosInventario movimiento = await _repositorio.ObtenerDatos(m => m.IdMovimiento == idMovimiento);

                if (movimiento == null)
                {
                    throw new TaskCanceledException("El movimiento no existe.");
                }

                bool respuesta = await _repositorio.Eliminar(movimiento);
                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar movimiento: " + ex.Message);
            }
        }

        public async Task<MovimientoInventarioDTO> ObtenerPorId(int idMovimiento)
        {
            try
            {
                IQueryable<MovimientosInventario> query = await _repositorio.Consultar(m => m.IdMovimiento == idMovimiento);
                MovimientosInventario movimiento = await query
                    .Include(m => m.IdProductoNavigation)
                    .Include(m => m.IdUsuarioNavigation)
                    .FirstOrDefaultAsync();

                if (movimiento == null)
                    throw new TaskCanceledException("El movimiento no existe.");

                return _mapper.Map<MovimientoInventarioDTO>(movimiento);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el movimiento: " + ex.Message);
            }
        }
       

        private async Task ActualizarStockProducto(int idProducto, string tipoMovimiento, int cantidad)
        {
            var producto = await _productoRepository.ObtenerDatos(p => p.IdProducto == idProducto);

            if (producto == null)
                throw new Exception("El producto especificado no existe.");

            if (tipoMovimiento.Equals("Entrada", StringComparison.OrdinalIgnoreCase))
            {
                producto.Stock = SumarStock(producto.Stock, cantidad);
            }
            else if (tipoMovimiento.Equals("Salida", StringComparison.OrdinalIgnoreCase))
            {
                producto.Stock = RestarStock(producto.Stock, cantidad);
            }
            else
            {
                throw new Exception("Tipo de movimiento inválido. Solo se permite 'Entrada' o 'Salida'.");
            }

            bool actualizado = await _productoRepository.Editar(producto);
            if (!actualizado)
                throw new Exception("Error al actualizar las existencias del producto en la base de datos.");
        }

        private int SumarStock(int stockActual, int cantidad)
        {
            return stockActual + cantidad;
        }

        private int RestarStock(int stockActual, int cantidad)
        {
            if (stockActual < cantidad)
                throw new Exception($"Stock insuficiente en el sistema. Disponibles: {stockActual}, Solicitados: {cantidad}.");

            return stockActual - cantidad;
        }
    }
}