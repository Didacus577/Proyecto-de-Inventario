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
              
                UsuarioDTO loginDto = new UsuarioDTO
                {
                    Correo = modelo.Correo,
                    Clave = modelo.Clave
                };

            
                var usuarioEncontrado = await _loginService.Login(loginDto);

                if (usuarioEncontrado == null)
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = "Correo o contraseña incorrectos.";
                    return Ok(respuesta);
                }

              
                string tokenGenerado = _authService.GenerarToken(usuarioEncontrado);

              
                var vmUsuario = _mapper.Map<VMUsuarios>(usuarioEncontrado);

                
                HttpContext.Session.SetInt32("IdRol", usuarioEncontrado.IdRol);
                HttpContext.Session.SetString("Token", tokenGenerado);
                HttpContext.Session.SetString("NombreUsuario", usuarioEncontrado.NombreUsuario);

               
                respuesta.Estado = true;
                respuesta.Objeto = vmUsuario;
                respuesta.Token = tokenGenerado;
                respuesta.Url = Url.Action("Index", "Home"); 

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
               
                respuesta.Estado = false;
                respuesta.Mensaje = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Salir()
        {
           
            HttpContext.Session.Clear();
           
            return RedirectToAction("IniciaSesion", "Login");
        }
    }
}