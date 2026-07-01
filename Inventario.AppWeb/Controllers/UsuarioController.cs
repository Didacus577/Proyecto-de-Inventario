using AutoMapper;
using Inventario.AppWeb.Models.ViewModels;
using Inventario.AppWeb.Utilidades.Response;
using Inventario.BLL.Interfaces;
using Inventario.DTOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.AppWeb.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Usuario")]
    public class UsuarioController : Controller
    {
        private readonly IUsuariosService _UsuarioService;
        private readonly IMapper _mapper;
        private readonly IUtilidadesService _utilidadesService;

        public UsuarioController(IUsuariosService usuariosService, IMapper mapper, IUtilidadesService utilidadesService)
        {
            _UsuarioService = usuariosService;
            _mapper = mapper;
            _utilidadesService = utilidadesService;
        }

      
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet("UsuarioLista")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var lista = await _UsuarioService.Lista();
                var listaVM = _mapper.Map<List<VMUsuarios>>(lista);
                return StatusCode(StatusCodes.Status200OK, new { data = listaVM });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [HttpGet("Obtener")]
        public async Task<IActionResult> Obtener(int idUsuario)
        {
            GenericResponse<VMUsuarios> response = new GenericResponse<VMUsuarios>();
            try
            {
                var dto = await _UsuarioService.ObtenerPorId(idUsuario);
                if (dto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { estado = false, mensaje = "Usuario no encontrado" });
                }

                response.Estado = true;
                response.Objeto = _mapper.Map<VMUsuarios>(dto);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

     
        [HttpPost("CrearUsuario")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] VMUsuarios modelo)
        {
            try
            {
                if (!string.IsNullOrEmpty(modelo.Clave))
                    modelo.Clave = _utilidadesService.ConvertirSHA256(modelo.Clave);

                var dto = _mapper.Map<UsuarioDTO>(modelo);
                var usuarioCreado = await _UsuarioService.Crear(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = true,
                    mensaje = "Usuario creado exitosamente",
                    data = _mapper.Map<VMUsuarios>(usuarioCreado)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        [HttpPut("EditarUsuario")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Editar([FromBody] VMUsuarios modelo)
        {
            try
            {
                if (!string.IsNullOrEmpty(modelo.Clave))
                {
                    modelo.Clave = _utilidadesService.ConvertirSHA256(modelo.Clave);
                }
                else
                {
                    modelo.Clave = null;
                }

                var dto = _mapper.Map<UsuarioDTO>(modelo);
                bool respuesta = await _UsuarioService.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = respuesta,
                    mensaje = respuesta ? "Usuario actualizado exitosamente" : "No se pudo actualizar el usuario"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

       
        [HttpDelete("EliminarUsuario")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            try
            {
                bool eliminado = await _UsuarioService.Eliminar(idUsuario);

                if (eliminado)
                {
                    return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Usuario eliminado correctamente" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { estado = false, mensaje = "No se pudo eliminar el usuario" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}