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
    [Route("UnidadMedida")]
    public class UnidadesMedidaController : Controller
    {
        private readonly IUnidadesService _unidadService;
        private readonly IMapper _mapper;

        public UnidadesMedidaController(IUnidadesService unidadService, IMapper mapper)
        {
            _unidadService = unidadService;
            _mapper = mapper;
        }

  
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

     
        [HttpGet("MedidaLista")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var lista = await _unidadService.Lista();
                var listaVM = _mapper.Map<List<VMUnidades>>(lista);
                return StatusCode(StatusCodes.Status200OK, new { data = listaVM });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

      
        [HttpPost("CrearMedida")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearMedida([FromBody] VMUnidades vmUnidad)
        {
            try
            {
                var dto = _mapper.Map<UnidadMedidaDTO>(vmUnidad);
                var medidaCreada = await _unidadService.Crear(dto);
                var vm = _mapper.Map<VMUnidades>(medidaCreada);

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Unidad de medida creada correctamente.", data = vm });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [HttpPut("EditarMedida")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditarMedida([FromBody] VMUnidades vmMedida)
        {
            try
            {
                var dto = _mapper.Map<UnidadMedidaDTO>(vmMedida);
                bool respuesta = await _unidadService.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = respuesta,
                    mensaje = respuesta ? "Unidad de medida actualizada correctamente." : "No se pudo actualizar la unidad de medida."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

      
        [HttpDelete("EliminarMedida")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int idMedida)
        {
            try
            {
                bool eliminado = await _unidadService.Eliminar(idMedida);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = eliminado,
                    mensaje = eliminado ? "Unidad de medida eliminada correctamente." : "No se pudo eliminar la unidad de medida."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}