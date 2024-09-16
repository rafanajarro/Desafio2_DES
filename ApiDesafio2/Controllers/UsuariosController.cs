using ApiDesafio2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var dbUserCorreo = dbContext.Usuarios.FirstOrDefault(x => x.CorreoElectronico == usuarioDto.CorreoElectronico);

            if (dbUserCorreo == null)
            {
                dbContext.Usuarios.Add(new Usuario
                {
                    NombreUsuario = codigoUsername,
                    Password = _passwordHasher.HashPassword(null, usuarioDto.Password),
                    CorreoElectronico = usuarioDto.CorreoElectronico,
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
        public IActionResult Login(LoginDto loginDto)
        {
            var usuario = dbContext.Usuarios.FirstOrDefault(x => x.NombreUsuario == loginDto.NombreUsuario);
            if (usuario != null)
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, loginDto.Password);

                if (resultado == PasswordVerificationResult.Success)
                {
                    return Ok(new
                    {
                        Message = "Logueado correctamente.",
                        NombreUsuario = usuario.NombreUsuario,
                        CorreoElectronico = usuario.CorreoElectronico,
                        RolUsuario = usuario.RolUsuario,
                        Nombre = usuario.Nombre,
                        Apellidos = usuario.Apellidos
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
                        usuario.CorreoElectronico,
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
