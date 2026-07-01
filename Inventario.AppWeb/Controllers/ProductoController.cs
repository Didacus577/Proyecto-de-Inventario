using AutoMapper;
using Inventario.AppWeb.Models.ViewModels;
using Inventario.BLL.Interfaces;
using Inventario.DTOS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin,operador")]
[Route("Producto")]
public class ProductoController : Controller
{
    private readonly IProductosService _productosServices;
    private readonly IMapper _mapper;

    public ProductoController(IProductosService productos, IMapper mapper)
    {
        _mapper = mapper;
        _productosServices = productos;
    }   

    [AllowAnonymous]
    [HttpGet("Administrador")]
    public IActionResult Administrador()
    {
        return View("AdminProducto");
    }

    [AllowAnonymous]
    [HttpGet("Operador")]
    public IActionResult Operador()
    {
        return View("OperadorProducto");
    }

    [HttpGet("ListarProductos")]
    [AllowAnonymous]
    public async Task<IActionResult> ListarProductos()
    {
        try
        {
            var lista = await _productosServices.Lista();
            var listaVM = _mapper.Map<List<VMProducto>>(lista);
            return StatusCode(StatusCodes.Status200OK, new { data = listaVM });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
        }
    }

    [Authorize(Roles = "admin,operador")]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear([FromBody] VMProducto modelo)
    {
        try
        {
            var dto = _mapper.Map<ProductosDTO>(modelo);
            var productoCreado = await _productosServices.Crear(dto);
            return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Producto creado con éxito" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
        }
    }

    [Authorize(Roles = "admin,operador")]
    [HttpPut("Editar")]
    public async Task<IActionResult> Editar([FromBody] VMProducto modelo)
    {
        try
        {
            var dto = _mapper.Map<ProductosDTO>(modelo);
            bool respuesta = await _productosServices.Editar(dto);
            return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Producto editado correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(int idProducto)
    {
        try
        {
            bool eliminado = await _productosServices.Eliminar(idProducto);
            return StatusCode(StatusCodes.Status200OK, new { estado = true, mensaje = "Producto eliminado" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
        }
    }

    [HttpGet("Obtener")]
    public async Task<IActionResult> Obtener(int idProducto)
    {
        try
        {
            var productoDto = await _productosServices.ObtenerPorId(idProducto);
            return StatusCode(StatusCodes.Status200OK, new { estado = true, objeto = _mapper.Map<VMProducto>(productoDto) });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { estado = false, mensaje = ex.Message });
        }
    }
}