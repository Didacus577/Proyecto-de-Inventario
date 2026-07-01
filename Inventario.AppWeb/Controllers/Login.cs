using AutoMapper;
using Inventario.AppWeb.Models.ViewModels;
using Inventario.AppWeb.Utilidades.Response;
using Inventario.BLL.Interfaces;
using Inventario.BLL.Servicios;
using Inventario.DTOS; 
using Microsoft.AspNetCore.Mvc;

namespace Inventario.AppWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;

        public LoginController(ILoginService loginService, IMapper mapper, AuthService authService)
        {
            _loginService = loginService;
            _mapper = mapper;
            _authService = authService;
        }

        public IActionResult IniciaSesion()
        {
            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] VMUsuarios modelo)
        {
            var respuesta = new GenericResponse<VMUsuarios>();

            try
            {
                // 1. Mapeamos el ViewModel que viene de la vista al DTO que espera el servicio
                UsuarioDTO loginDto = new UsuarioDTO
                {
                    Correo = modelo.Correo,
                    Clave = modelo.Clave
                };

                // 2. Llamamos al servicio pasando el DTO
                var usuarioEncontrado = await _loginService.Login(loginDto);

                if (usuarioEncontrado == null)
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = "Correo o contraseña incorrectos.";
                    return Ok(respuesta);
                }

                // 3. Generar Token usando el DTO encontrado
                string tokenGenerado = _authService.GenerarToken(usuarioEncontrado);

                // 4. Mapear el resultado a ViewModel para devolverlo a la vista
                var vmUsuario = _mapper.Map<VMUsuarios>(usuarioEncontrado);

                // 5. Guardar en sesión (Si estás usando Session Middleware)
                // Asegúrate de que las propiedades coincidan con tu DTO
                HttpContext.Session.SetInt32("IdRol", usuarioEncontrado.IdRol);
                HttpContext.Session.SetString("Token", tokenGenerado);
                HttpContext.Session.SetString("NombreUsuario", usuarioEncontrado.NombreUsuario);

                // 6. Configurar respuesta exitosa
                respuesta.Estado = true;
                respuesta.Objeto = vmUsuario;
                respuesta.Token = tokenGenerado;
                respuesta.Url = Url.Action("Index", "Home"); // Genera la URL de forma segura

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                // Devolvemos el error dentro del formato GenericResponse para que el JS lo maneje
                respuesta.Estado = false;
                respuesta.Mensaje = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Salir()
        {
            // Limpiar sesión
            HttpContext.Session.Clear();

            // Aquí podrías agregar la lógica para desloguear de Authentication Cookies si las usas
            return RedirectToAction("IniciaSesion", "Login");
        }
    }
}