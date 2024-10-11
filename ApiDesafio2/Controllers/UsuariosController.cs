using ApiDesafio2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiDesafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ProyectoDbContext dbContext;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuariosController(ProyectoDbContext dbContext)
        {
            this.dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UsuarioDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Codigo de username
            string codigoUsername = GenerarUsername(usuarioDto.Apellidos);
            var dbUsername = dbContext.Usuarios.FirstOrDefault(x => x.NombreUsuario == codigoUsername);

            while (dbUsername != null)
            {
                codigoUsername = GenerarUsername(usuarioDto.Apellidos);
            }

            //Verificar no exista un usuario con otro correo
            var dbUserCorreo = dbContext.Usuarios.FirstOrDefault(x => x.Email == usuarioDto.CorreoElectronico);

            if (dbUserCorreo == null)
            {
                dbContext.Usuarios.Add(new Usuario
                {
                    NombreUsuario = codigoUsername,
                    PasswordHash = _passwordHasher.HashPassword(null, usuarioDto.Password),
                    Email = usuarioDto.CorreoElectronico,
                    Telefono = usuarioDto.Telefono,
                    Nombre = usuarioDto.Nombre,
                    Apellidos = usuarioDto.Apellidos,
                    RolUsuario = usuarioDto.RolUsuario,
                    FechaNacimiento = usuarioDto.FechaNacimiento
                });
                dbContext.SaveChanges();
                return Ok(new
                {
                    message = "El usuario ha sido creado con exito.",
                    username = codigoUsername
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Ya existe un usuario con ese correo electronico.",
                });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var usuario = dbContext.Usuarios.FirstOrDefault(x => x.NombreUsuario == loginDto.NombreUsuario);
            if (usuario != null)
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, loginDto.Password);

                if (resultado == PasswordVerificationResult.Success)
                {                    
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginDto.NombreUsuario),
                        new Claim(ClaimTypes.Email, loginDto.Password),                 
                    };
                    
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);                    
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, 
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Tiempo de expiración de la cookie
                    };

                    // Genera la cookie de autenticación
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                    return Ok(new
                    {
                        Message = "Logueado correctamente.",
                        Usuario = new
                        {
                            usuario.NombreUsuario,
                            usuario.Email,
                            usuario.RolUsuario,
                            usuario.Nombre,
                            usuario.Apellidos
                        }
                    });
                }
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            var usuario = dbContext.Usuarios.FirstOrDefault(x => x.UsuarioId == id);
            if (usuario != null)
            {
                return Ok(new
                {
                    message = "Usuario encontrado.",
                    user = new
                    {
                        usuario.NombreUsuario,
                        usuario.Email,
                        usuario.RolUsuario,
                        usuario.Nombre,
                        usuario.Apellidos
                    }
                });
            }
            else
            {
                return NoContent();
            }
        }

        public static string GenerarUsername(string ApellidosUsuario)
        {
            //Obtener iniciales apellidos            
            string iniciales = "";
            var palabras = ApellidosUsuario.Split(' ');
            if (palabras.Length >= 2)
                iniciales = palabras[0][0].ToString().ToUpper() + palabras[1][0].ToString().ToUpper();
            else
                iniciales = palabras[0][0].ToString().ToUpper() + palabras[0][0].ToString().ToUpper();

            string digitos = "0123456789";
            Random random = new Random();
            char[] resultado = new char[8];

            // Generar 8 caracteres aleatorios
            for (int i = 0; i < resultado.Length; i++)
            {
                resultado[i] = digitos[random.Next(digitos.Length)];
            }

            string stringAleatorio = new string(resultado);
            string codigoUsername = iniciales + stringAleatorio;
            return codigoUsername;
        }
    }
}
