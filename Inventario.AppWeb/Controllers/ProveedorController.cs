using AutoMapper;
using Inventario.AplicacionWeb.Models.ViewModels;
using Inventario.BLL.Interfaces;
using Inventario.DTOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.AppWeb.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Proveedor")]
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedoresServices;
        private readonly IMapper _mapper;

        public ProveedorController(IProveedorService proveedores, IMapper mapper)
        {
            _mapper = mapper;
            _proveedoresServices = proveedores;
        }

      
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

      
        [HttpGet("ListaProveedor")]
        public async Task<IActionResult> ListaProveedores()
        {
            try
            {
                var lista = await _proveedoresServices.Lista();
                var vmLista = _mapper.Map<List<VMProveedor>>(lista);

                return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

     
        [HttpPost("CrearProveedor")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearProveedor([FromBody] VMProveedor modelo)
        {
            try
            {
                var entidad = _mapper.Map<ProveedoresDTO>(modelo);
                var proveedorCreado = await _proveedoresServices.Crear(entidad);
                var vmProveedor = _mapper.Map<VMProveedor>(proveedorCreado);

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Proveedor creado correctamente.", data = vmProveedor });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [HttpPut("EditarProveedor")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditarProveedor([FromBody] VMProveedor modelo)
        {
            try
            {
                var dto = _mapper.Map<ProveedoresDTO>(modelo);
                bool respuesta = await _proveedoresServices.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = respuesta,
                    mensaje = respuesta ? "Proveedor editado correctamente." : "No se pudo editar el proveedor."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

       
        [HttpDelete("EliminarProveedor")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarProveedor(int idProveedor)
        {
            try
            {
                bool eliminado = await _proveedoresServices.Eliminar(idProveedor);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = eliminado,
                    mensaje = eliminado ? "Proveedor eliminado correctamente." : "No se pudo eliminar el proveedor."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}