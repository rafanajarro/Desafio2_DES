using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebsiteDesafio2.Models;

namespace WebsiteDesafio2.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;
        private readonly EmailService _emailService;
        private readonly string urlApi = "https://localhost:7042/api/Usuarios";

        public AuthController(ApiService apiService, EmailService emailService)
        {
            _apiService = apiService;
            _emailService = emailService;
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

                if (!string.IsNullOrEmpty(respuestaLogin))
                {
                    var respuesta = JsonConvert.DeserializeObject<RespuestaLoginDto>(respuestaLogin);

                    if (respuesta != null && respuesta.message == "Logueado correctamente.")
                    {
                        // Guardar los datos en la sesión
                        HttpContext.Session.SetString("NombreUsuario", respuesta.usuario.NombreUsuario);
                        HttpContext.Session.SetString("CorreoElectronico", respuesta.usuario.CorreoElectronico);
                        HttpContext.Session.SetString("RolUsuario", respuesta.usuario.RolUsuario);
                        HttpContext.Session.SetString("Nombre", respuesta.usuario.Nombre);
                        HttpContext.Session.SetString("Apellidos", respuesta.usuario.Apellidos);

                        // Redirigir al Dashboard
                        return RedirectToAction("Index", "OfertaEmpleos");
                    }
                    else
                    {
                        ViewBag.Error = "Error en el inicio de sesión.";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.Error = "Credenciales incorrectas.";
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
                    Console.WriteLine("respuestaLogin");
                    var respuesta = JsonConvert.DeserializeObject<RespuestaRegisterDto>(respuestaLogin);
                    if (respuesta != null)
                    {
                        Console.WriteLine(respuesta.username);
                        Console.WriteLine(datosRegister.correoElectronico);
                        bool correoEnviado = await _emailService.EnviarCorreoAsync(datosRegister.correoElectronico,
                            respuesta.username);
                        if (correoEnviado)
                        {
                            ViewBag.Error = "Usuario creado con éxito. Revisar correo";
                        }
                        else
                        {
                            ViewBag.Error = "Usuario creado con éxito, pero no se pudo enviar el correo.";
                        }
                        return View("Login");
                    }
                    else
                    {
                        ViewBag.Error = "La respuesta del servidor esta vacía.";
                        return View("Register");
                    }
                }
                else
                {
                    ViewBag.Error = "Ya existe un usuario con ese correo electronico.";
                    return View("Register");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al completar la solicitud. Revisa los datos ingresados";
                return View("Register");
            }
        }
    }
}
