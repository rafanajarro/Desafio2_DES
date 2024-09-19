using ApiDesafio2.Controllers;
using ApiDesafio2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApi_Desafio2
{
    public class OfertaEmpleosController
    {
        [Fact]
        public void CrearOferta_OfertaValida()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new OfertasEmpleoController(context);

            var ofertaEmpleo = new OfertaEmpleo
            {
                OfertaId = 1,
                NombreOferta = "Desarrollador",
                DescripcionOferta = "Desarrollador de software con experiencia.",
                Requisitos = "Experiencia en C#",
                Salario = 50000,
                Contacto = "contacto@empresa.com",
                UsuarioId = "usuario1"
            };

            // Act
            var result = controller.CrearOferta(ofertaEmpleo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Console.WriteLine(response);
            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            // Verifica los valores esperados
            Assert.Equal("La oferta ha sido creada con exito.", messageProperty);
        }

        [Fact]
        public void GetMisOfertas_UsuarioConOfertas()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new OfertasEmpleoController(context);

            // Agregar oferta para el usuario
            context.OfertasEmpleo.Add(new OfertaEmpleo
            {
                OfertaId = 1,
                NombreOferta = "Desarrollador",
                DescripcionOferta = "Desarrollador de software con experiencia.",
                Requisitos = "Experiencia en C#",
                Salario = 50000,
                FechaPublicacion = DateTime.Now,
                Contacto = "contacto@empresa.com",
                UsuarioId = "usuario1"
            });
            context.SaveChanges();

            // Act
            var result = controller.GetMisOfertas("usuario1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Console.WriteLine("----------" + response);
            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            // Verifica los valores esperados
            Assert.Equal("Ofertas encontradas.", messageProperty);
        }

        [Fact]
        public void UpdateOferta_OfertaExistente()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new OfertasEmpleoController(context);

            // Agregar oferta
            context.OfertasEmpleo.Add(new OfertaEmpleo
            {
                OfertaId = 1,
                NombreOferta = "Desarrollador",
                DescripcionOferta = "Desarrollador de software con experiencia.",
                Requisitos = "Experiencia en C#",
                Salario = 50000,
                FechaPublicacion = DateTime.Now,
                Contacto = "contacto@empresa.com",
                UsuarioId = "usuario1"
            });
            context.SaveChanges();

            var ofertaActualizada = new OfertaEmpleo
            {
                NombreOferta = "Senior Desarrollador",
                DescripcionOferta = "Desarrollador de software con mucha experiencia.",
                Requisitos = "Experiencia en C# y .NET",
                Salario = 70000,
                FechaPublicacion = DateTime.Now,
                Contacto = "contacto@empresa.com",
                UsuarioId = "usuario1"
            };

            // Act
            var result = controller.UpdateOferta(1, ofertaActualizada);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Console.WriteLine("----------" + response);
            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            // Verifica los valores esperados
            Assert.Equal("Oferta actualizada correctamente.", messageProperty);
        }

        [Fact]
        public void EliminarOferta_OfertaExistente()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new OfertasEmpleoController(context);

            // Agregar oferta
            context.OfertasEmpleo.Add(new OfertaEmpleo
            {
                OfertaId = 1,
                NombreOferta = "Desarrollador",
                DescripcionOferta = "Desarrollador de software con experiencia.",
                Requisitos = "Experiencia en C#",
                Salario = 50000,
                FechaPublicacion = DateTime.Now,
                Contacto = "contacto@empresa.com",
                UsuarioId = "usuario1"
            });
            context.SaveChanges();

            // Act
            var result = controller.EliminarOferta(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Console.WriteLine("----------" + response);
            // Accede directamente a las propiedades del tipo anónimo
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            // Verifica los valores esperados
            Assert.Equal("Oferta eliminada correctamente.", messageProperty);
            // Verifica que la oferta fue eliminada de la base de datos
            var oferta = context.OfertasEmpleo.FirstOrDefault(o => o.OfertaId == 1);
            Assert.Null(oferta);            
        }
    }
}
