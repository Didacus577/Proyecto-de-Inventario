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
    [Route("Categoria")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _CategoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
        {
            _CategoriaService = categoriaService;
            _mapper = mapper;
        }

       
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet("ListarCategorias")]
        public async Task<IActionResult> ListarCategorias()
        {
            try
            {
                var lista = await _CategoriaService.Lista();
                var listaVM = _mapper.Map<List<VMCategoria>>(lista);
                return StatusCode(StatusCodes.Status200OK, new { data = listaVM });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    
        [HttpGet("ObtenerCategoria")]
        public async Task<IActionResult> Obtener(int idCategoria)
        {
            try
            {
                var dto = await _CategoriaService.ObtenerPorId(idCategoria);

                if (dto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { estado = false, mensaje = "No encontrada" });
                }

                var vm = _mapper.Map<VMCategoria>(dto);
                return StatusCode(StatusCodes.Status200OK, new { estado = true, objeto = vm });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [HttpPost("CrearCategoria")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] VMCategoria modelo)
        {
            try
            {
                var dto = _mapper.Map<CategoriaDTO>(modelo);
                var categoriaCreada = await _CategoriaService.Crear(dto);
                var vmCategoria = _mapper.Map<VMCategoria>(categoriaCreada);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = true,
                    mensaje = "Categoría creada correctamente.",
                    data = vmCategoria
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [HttpPut("EditarCategoria")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Editar([FromBody] VMCategoria modelo)
        {
            try
            {
                CategoriaDTO dto = _mapper.Map<CategoriaDTO>(modelo);
                bool respuesta = await _CategoriaService.Editar(dto);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = respuesta,
                    mensaje = respuesta ? "Categoría editada correctamente." : "No se pudo editar."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }

        
        [HttpDelete("CategoriaEliminar")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int idCategoria)
        {
            try
            {
                bool eliminado = await _CategoriaService.Eliminar(idCategoria);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    estado = eliminado,
                    mensaje = eliminado ? "Categoría eliminada correctamente." : "No se pudo eliminar."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
            }
        }
    }
}