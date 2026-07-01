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
    [Route("Marca")]
    public class MarcaController : Controller
    {
        private readonly IMarcaService _marcaService;
        private readonly IMapper _mapper;

        public MarcaController(IMarcaService marcaService, IMapper mapper)
        {
            _marcaService = marcaService;
            _mapper = mapper;
        }

     
        [AllowAnonymous]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet("ListaMarca")]
        public async Task<IActionResult> ListaMarcas()
        {
            try
            {
                var lista = await _marcaService.Lista();
                var vmLista = _mapper.Map<List<VMMarca>>(lista);

                return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        
        [HttpGet("Obtener")]
        public async Task<IActionResult> Obtener(int IdMarca)
        {
            try
            {
                var lista = await _marcaService.Lista();
                var marca = lista.FirstOrDefault(x => x.IdMarca == IdMarca);

                if (marca == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { estado = false, mensaje = "Marca no encontrada." });
                }

                var vmMarca = _mapper.Map<VMMarca>(marca);
                return StatusCode(StatusCodes.Status200OK, new { estado = true, objeto = vmMarca });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

      
        [HttpPost("CrearMarca")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] VMMarca modelo)
        {
            try
            {
                var dto = _mapper.Map<MarcasDTO>(modelo);
                var marcaCreada = await _marcaService.Crear(dto);
                var vmMarca = _mapper.Map<VMMarca>(marcaCreada);

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Marca creada correctamente.", data = vmMarca });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

      
        [HttpPut("EditarMarca")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Editar([FromBody] VMMarca modelo)
        {
            try
            {
                MarcasDTO dto = _mapper.Map<MarcasDTO>(modelo);
                bool respuesta = await _marcaService.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = respuesta,
                    mensaje = respuesta ? "Marca editada correctamente." : "No se pudo editar la marca."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

       
        [HttpDelete("EliminarMarca")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int idMarca)
        {
            try
            {
                bool eliminado = await _marcaService.Eliminar(idMarca);

                if (!eliminado)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { estado = false, mensaje = "No se pudo eliminar la marca." });
                }

                return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Marca eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}