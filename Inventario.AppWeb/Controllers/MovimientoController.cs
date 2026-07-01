using AutoMapper;
using Inventario.AppWeb.Models.ViewModels;
using Inventario.BLL.Interfaces;
using Inventario.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.AppWeb.Controllers
{
   
    [Authorize(Roles = "admin,operador")]
    [Route("Movimiento")]
    public class MovimientoController : Controller
    {
        private readonly IMovimientoService _movimientoService;
        private readonly IMapper _mapper;

        public MovimientoController(IMovimientoService movimientoService, IMapper mapper)
        {
            _mapper = mapper;
            _movimientoService = movimientoService;
        }

        
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("VerMovimiento")]
        public IActionResult VerMovimiento()
        {
            return View();
        }

       
        [Authorize(Roles = "admin,operador")]
        [HttpGet("ListarHistorial")]
        public async Task<IActionResult> ListarHistorial()
        {
            try
            {
                var lista = await _movimientoService.Lista();
                var vmLista = _mapper.Map<List<VMMovimiento>>(lista);

                return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [Authorize(Roles = "admin,operador")]
        [HttpGet("Obtener")]
        public async Task<IActionResult> Obtener(int idMovimiento)
        {
            try
            {
                var dto = await _movimientoService.ObtenerPorId(idMovimiento);

                if (dto == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { estado = false, mensaje = "Movimiento no encontrado." });

                var vm = _mapper.Map<VMMovimiento>(dto);
                return StatusCode(StatusCodes.Status200OK, new { estado = true, objeto = vm });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        [Authorize(Roles = "admin,operador")]
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] VMMovimiento modelo)
        {
            try
            {
                var dto = _mapper.Map<MovimientoInventarioDTO>(modelo);
                var movimientoCreado = await _movimientoService.Crear(dto);
                var vmMovimiento = _mapper.Map<VMMovimiento>(movimientoCreado);

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Movimiento creado correctamente.", data = vmMovimiento });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        [Authorize(Roles = "admin,operador")]
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] VMMovimiento modelo)
        {
            try
            {
                MovimientoInventarioDTO dto = _mapper.Map<MovimientoInventarioDTO>(modelo);
                bool respuesta = await _movimientoService.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Movimiento editado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [Authorize(Roles = "admin")]
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Eliminar(int idMovimiento)
        {
            try
            {
                bool eliminado = await _movimientoService.Eliminar(idMovimiento);

                if (!eliminado)
                    return StatusCode(StatusCodes.Status400BadRequest, new { estado = false, mensaje = "No se pudo eliminar el movimiento." });

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Movimiento eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}