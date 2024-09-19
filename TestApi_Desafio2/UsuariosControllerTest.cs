using ApiDesafio2.Controllers;
using ApiDesafio2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApi_Desafio2
{
    public class UsuariosControllerTest
    {
        [Fact]
        public async Task Register_ValidUser()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            var usuarioDto = new UsuarioDTO
            {
                Password = "Test123",
                CorreoElectronico = "test@example.com",
                Telefono = "123456789",
                Nombre = "Juan",
                Apellidos = "Perez Lopez",
                RolUsuario = "Solicitante",
                FechaNacimiento = DateTime.Now
            };

            // Act
            var result = controller.Register(usuarioDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Verifica que sea un OkObjectResult
            var response = okResult.Value;

            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            var usernameProperty = response.GetType().GetProperty("username").GetValue(response, null);

            // Verifica los valores esperados
            Assert.Equal("El usuario ha sido creado con exito.", messageProperty);
            Assert.StartsWith("PL", usernameProperty.ToString());
        }

        [Fact]
        public async Task Register_DuplicateEmail()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            context.Usuarios.Add(new ApiDesafio2.Models.Usuario
            {
                NombreUsuario = "username",
                Password = "Test123",
                CorreoElectronico = "test@example.com",
                Telefono = "123456789",
                Nombre = "Juan",
                Apellidos = "Perez Lopez",
                RolUsuario = "Solicitante",
                FechaNacimiento = DateTime.Now
            });
            context.SaveChanges();

            var usuarioDto = new UsuarioDTO
            {
                Apellidos = "Perez Lopez",
                CorreoElectronico = "test@example.com",
                Password = "Test123",
                Telefono = "123456789",
                Nombre = "Juan",
                RolUsuario = "User",
                FechaNacimiento = DateTime.Now
            };

            // Act
            var result = controller.Register(usuarioDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            Console.WriteLine(badRequestResult.StatusCode);

            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);

            // Verifica los valores esperados
            Assert.Equal("Ya existe un usuario con ese correo electronico.", messageProperty);
        }

        [Fact]
        public async Task Login_ValidCredentialst()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            var passwordHasher = new PasswordHasher<ApiDesafio2.Models.Usuario>();
            var usuario = new ApiDesafio2.Models.Usuario
            {
                NombreUsuario = "testuser",
                CorreoElectronico = "test@example.com",
                Password = passwordHasher.HashPassword(null, "Test123"),
                Nombre = "Juan",
                Apellidos = "Perez",
                RolUsuario = "User",
                Telefono = "123456789",
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var loginDto = new ApiDesafio2.Models.LoginDto
            {
                NombreUsuario = "testuser",
                Password = "Test123"
            };

            // Act
            var result = controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Console.WriteLine("--------------- " + response);
            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("Message").GetValue(response, null);
            // Verifica los valores esperados
            Assert.Equal("Logueado correctamente.", messageProperty);
        }

        [Fact]
        public async Task Login_InvalidPassword()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            // Pre-seed database with a user
            var passwordHasher = new PasswordHasher<ApiDesafio2.Models.Usuario>();
            var usuario = new ApiDesafio2.Models.Usuario
            {
                NombreUsuario = "testuser",
                CorreoElectronico = "test@example.com",
                Password = passwordHasher.HashPassword(null, "Test123"),
                Nombre = "Juan",
                Apellidos = "Perez",
                RolUsuario = "User",
                Telefono = "123456789",
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var loginDto = new ApiDesafio2.Models.LoginDto
            {
                NombreUsuario = "testuser",
                Password = "WrongPassword"
            };

            // Act
            var result = controller.Login(loginDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetUser_ValidId()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            // Pre-seed database with a user
            var usuario = new Usuario
            {
                UsuarioId = 1,
                NombreUsuario = "testuser",
                Password = "Test123",
                CorreoElectronico = "test@example.com",
                Telefono = "123456789",
                Nombre = "Juan",
                Apellidos = "Perez",
                RolUsuario = "User"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Act
            var result = controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Console.WriteLine(response);
            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            // Verifica los valores esperados
            Assert.Equal("Usuario encontrado.", messageProperty);
        }

        [Fact]
        public async Task GetUser_InvalidId()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new UsuariosController(context);

            // Act
            var result = controller.GetUser(999);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
