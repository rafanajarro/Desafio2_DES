using Microsoft.AspNetCore.Mvc;
using WebsiteDesafio2.Models;

namespace WebsiteDesafio2.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;
        private readonly string urlApi = "https://localhost:7042/api/Usuarios";

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var datosLogin = new
            {
                nombreUsuario = loginDto.NombreUsuario,
                password = loginDto.Password
            };

            try
            {
                // Consumir la API y obtener la respuesta
                var respuestaLogin = await _apiService.EnviarDatosALaApi(urlApi + "/Login", datosLogin);

                // Evaluar la respuesta recibida
                if (respuestaLogin.Length > 0)
                {
                    // Login exitoso, puedes redirigir al usuario
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    // Mostrar un mensaje de error en la vista
                    ViewBag.Error = "Usuario o contraseña incorrectos";
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo si la API no responde
                ViewBag.Error = "Ocurrió un error al iniciar sesión: " + ex.Message;
                return View("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsuarioDto usuarioDTO)
        {
            var datosRegister = new
            {

                password = usuarioDTO.Password,
                correoElectronico = usuarioDTO.CorreoElectronico,
                telefono = usuarioDTO.Telefono,
                nombre = usuarioDTO.Nombre,
                apellidos = usuarioDTO.Apellidos,
                rolUsuario = usuarioDTO.RolUsuario,
                fechaNacimiento = usuarioDTO.FechaNacimiento
            };

            try
            {
                var respuestaLogin = await _apiService.EnviarDatosALaApi(urlApi + "/Register", datosRegister);

                if (respuestaLogin.Length > 1)
                {
                    ViewBag.Error = "Usuario creado con exito.";
                    return View("Login");
                }
                else
                {
                    ViewBag.Error = "Ya existe un usuario con ese correo electronico.";
                    return View("Register");
                }
            }
            catch (Exception ex)
            {                
                ViewBag.Error = "Ocurrió un error al completar la solicitud: " + ex.Message;
                return View("Register");
            }
        }
    }
}
