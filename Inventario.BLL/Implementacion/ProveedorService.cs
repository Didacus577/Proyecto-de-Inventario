using AutoMapper;
using Inventario.BLL.Interfaces;
using Inventario.DAL.Interfaces;
using Inventario.DTOS;
using Inventario.Entity;
using Microsoft.EntityFrameworkCore;


namespace Inventario.BLL.Implementacion
{
    public class ProveedorService : IProveedorService
    {
        private readonly IGenericRepository<Proveedores> _repositorio;
        private readonly IMapper _mapper;

        public ProveedorService(IGenericRepository<Proveedores> repositorio,IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<List<ProveedoresDTO>> Lista()
        {
            try
            {
                IQueryable<Proveedores> query = await _repositorio.Consultar();
                var listaEntidades = query.ToList();

                if (listaEntidades == null)
                {
                    throw new TaskCanceledException("No se encontraron proveedores registrados en el sistema.");
                }

                return _mapper.Map<List<ProveedoresDTO>>(listaEntidades);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProveedoresDTO> Crear(ProveedoresDTO proveedoresDTO)
        {
            try
            {

                Proveedores existeNombre = await _repositorio.ObtenerDatos(p => p.NombreProveedor.ToLower() == proveedoresDTO.NombreProveedor.ToLower().Trim());

                if (existeNombre != null)
                {
                    throw new Exception("Ya existe un proveedor con ese nombre");

                }

                Proveedores entidad = _mapper.Map<Proveedores>(proveedoresDTO);
                Proveedores proveedorCreado = await _repositorio.Crear(entidad);

                if (proveedorCreado.IdProveedor == 0)
                {
                    throw new TaskCanceledException("No se pudo registrar el proveedor.");
                }

                return _mapper.Map<ProveedoresDTO>(proveedorCreado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el proveedor: " + ex.Message);
            }
        }

        public async Task<bool> Editar(ProveedoresDTO proveedoresDTO)
        {
            try
            {
                Proveedores existente = await _repositorio.ObtenerDatos(p => p.IdProveedor == proveedoresDTO.IdProveedor);                

                if (existente == null)
                {
                    throw new TaskCanceledException("El proveedor no existe.");
                }

                existente.NombreProveedor = proveedoresDTO.NombreProveedor;
                existente.Telefono = proveedoresDTO.Telefono;
                existente.Correo = proveedoresDTO.Correo;
                existente.Direccion = proveedoresDTO.Direccion;

                bool respuesta = await _repositorio.Editar(existente);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el proveedor.");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el proveedor: " + ex.Message);
            }
        }

        public async Task<bool> Eliminar(int idProveedor)
        {
            try
            {
                Proveedores proveedorEncontrado = await _repositorio.ObtenerDatos(p => p.IdProveedor == idProveedor);

                if (proveedorEncontrado == null)
                {
                    throw new TaskCanceledException("El proveedor no existe.");
                }

                bool respuesta = await _repositorio.Eliminar(proveedorEncontrado);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar el proveedor.");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el proveedor: " + ex.Message);
            }
        }

        public async Task<Proveedores> ObtenerPorId(int idProveedor)
        {
            IQueryable<Proveedores> query = await _repositorio.Consultar(p => p.IdProveedor == idProveedor);
            Proveedores proveedor = await query.FirstOrDefaultAsync();

            if (proveedor == null)
            {
                throw new TaskCanceledException("El proveedor no existe.");
            }

            return proveedor;
        }
    }
}
