using AutoMapper;
using Inventario.AppWeb.Models.ViewModels;
using Inventario.BLL.Interfaces;
using Inventario.DTOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.AppWeb.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Roles")]
    public class RolesController : Controller
    {
        private readonly IRolService _rolService;
        private readonly IMapper _mapper;

        public RolesController(IRolService rolService, IMapper mapper)
        {
            _rolService = rolService;
            _mapper = mapper;
        }

      
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ListaRoles")]
        public async Task<IActionResult> ListaRoles()
        {
            try
            {
                var lista = await _rolService.Lista();
                var listaVM = _mapper.Map<List<VMRoles>>(lista);
                return StatusCode(StatusCodes.Status200OK, new { data = listaVM });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

    
        [HttpGet("ObtenerRol")]
        public async Task<IActionResult> ObtenerRol(int idRol)
        {
            try
            {
                var dto = await _rolService.ObtenerPorId(idRol);

                if (dto == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { estado = false, mensaje = "Rol no encontrado." });

                var vm = _mapper.Map<VMRoles>(dto);
                return StatusCode(StatusCodes.Status200OK, new { estado = true, objeto = vm });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

     
        [HttpPost("CrearRol")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearRol([FromBody] VMRoles modelo)
        {
            try
            {
                var dto = _mapper.Map<RolDTO>(modelo);
                var creado = await _rolService.Crear(dto);
                var vm = _mapper.Map<VMRoles>(creado);

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Rol creado correctamente.", data = vm });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

    
        [HttpPut("EditarRol")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditarRol([FromBody] VMRoles modelo)
        {
            try
            {
                if (modelo.IdRol <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new { estado = false, mensaje = "ID de rol inválido." });

                var dto = _mapper.Map<RolDTO>(modelo);
                bool respuesta = await _rolService.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = respuesta,
                    mensaje = respuesta ? "Rol actualizado correctamente." : "No se pudo actualizar el rol."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

       
        [HttpDelete("EliminarRol")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarRol(int idRol)
        {
            try
            {
                bool eliminado = await _rolService.Eliminar(idRol);

                if (!eliminado)
                    return StatusCode(StatusCodes.Status400BadRequest, new { estado = false, mensaje = "No se pudo eliminar el rol." });

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Rol eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}