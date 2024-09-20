using ApiDesafio2.Controllers;
using ApiDesafio2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApi_Desafio2
{
    public class HojaDeVidaControllerTest
    {
        [Fact]
        public void GetMisHojasDeVida_ValidUser_ReturnsOk()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            context.HojaDeVida.Add(new HojaDeVida { Id = 1, usuario = "testuser", NombreCompleto = "Juan Perez" });
            context.SaveChanges();
            var controller = new HojaDeVidaController(context);
            var result = controller.GetMisHojasDeVida("testuser");
            var okResult = Assert.IsType<OkObjectResult>(result);
            var hojas = Assert.IsAssignableFrom<IEnumerable<dynamic>>(okResult.Value.GetType().GetProperty("hojas").GetValue(okResult.Value));
            Assert.Single(hojas);
        }


        [Fact]
        public void EliminarHoja_ValidId_ReturnsOk()
        {
            var context = Setup.GetInMemoryDatabaseContext();
            var hojaDeVida = new HojaDeVida { Id = 1, usuario = "testuser", NombreCompleto = "Juan Perez" };
            context.HojaDeVida.Add(hojaDeVida);
            context.SaveChanges();
            var controller = new HojaDeVidaController(context);
            var result = controller.EliminarHoja(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
            Assert.Equal("Oferta eliminada correctamente.", message);
        }


        [Fact]
        public void agregarHojaDeVida_ValidData_ReturnsOk()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new HojaDeVidaController(context);

            // Crear una hoja de vida inicial (opcional, dependiendo de si es necesario)
            context.HojaDeVida.Add(new HojaDeVida
            {
                Id = 1, // Asignar un ID si es necesario
                NombreCompleto = "Juan Perez",
                FechaNacimiento = DateTime.Now,
                usuario = "testuser",
                FormacionesAcademicas = new List<FormacionAcademica>(),
                ExperienciasProfesionales = new List<ExperienciaProfesional>(),
                ReferenciasPersonales = new List<ReferenciaPersonal>(),
                Idiomas = new List<Idioma>()
            });
            context.SaveChanges();

            // Crear el DTO para la nueva hoja de vida
            var hojaDeVidaDto = new HojaDeVidaDTO
            {
                NombreCompleto = "Juan Perez Actualizado",
                FechaNacimiento = DateTime.Now.AddYears(-25),
                usuario = "testuser",
                FormacionesAcademicas = new List<FormacionAcademicaDTO>
        {
            new FormacionAcademicaDTO
            {
                TituloObtenido = "Ingeniero",
                Institucion = "Universidad XYZ",
                FechaInicio = DateTime.Now.AddYears(-4),
                FechaFin = DateTime.Now
            }
        },
                ExperienciasProfesionales = new List<ExperienciaProfesionalDTO>
        {
            new ExperienciaProfesionalDTO
            {
                Cargo = "Desarrollador",
                Descripcion = "Desarrollo de software",
                Empresa = "Empresa ABC",
                FechaInicio = DateTime.Now.AddYears(-2),
                FechaFin = DateTime.Now
            }
        },
                ReferenciasPersonales = new List<ReferenciaPersonalDTO>
        {
            new ReferenciaPersonalDTO
            {
                Nombre = "Carlos Garcia",
                Telefono = "987654321",
                Relacion = "Ex-jefe"
            }
        },
                Idiomas = new List<IdiomaDTO>
        {
            new IdiomaDTO
            {
                NombreIdioma = "Inglés",
                Nivel = "Intermedio"
            }
        }
            };

            // Act
            var result = controller.agregarHojaDeVida(hojaDeVidaDto); // Llama al método del controlador.

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Verifica que el resultado es un OkObjectResult.
            var response = okResult.Value; // Obtiene el valor del resultado.

            // Verifica que el mensaje sea correcto.
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);
            Assert.Equal("La Hoja de Vida ha sido creada con exito.", messageProperty);

        }


        [Fact]
        public void UpdateHojaDeVida_ValidData_ReturnsOk()
        {
            // Arrange
            var context = Setup.GetInMemoryDatabaseContext();
            var controller = new HojaDeVidaController(context);

            // Agregar una hoja de vida existente para actualizar
            var existingHojaDeVida = new HojaDeVida
            {
                Id = 1,
                NombreCompleto = "Juan Perez",
                FechaNacimiento = DateTime.Now.AddYears(-25),
                usuario = "testuser"
            };
            context.HojaDeVida.Add(existingHojaDeVida);
            context.SaveChanges();

            // DTO para actualizar
            var hojaDeVidaDto = new HojaDeVidaDTO
            {
                NombreCompleto = "Juan Perez Actualizado",
                FechaNacimiento = DateTime.Now.AddYears(-25),
                usuario = "testuser",
                FormacionesAcademicas = new List<FormacionAcademicaDTO>
        {
            new FormacionAcademicaDTO
            {
                TituloObtenido = "Ingeniero",
                Institucion = "Universidad XYZ",
                FechaInicio = DateTime.Now.AddYears(-4),
                FechaFin = DateTime.Now
            }
        },
                ExperienciasProfesionales = new List<ExperienciaProfesionalDTO>
        {
            new ExperienciaProfesionalDTO
            {
                Cargo = "Desarrollador",
                Descripcion = "Desarrollo de software",
                Empresa = "Empresa ABC",
                FechaInicio = DateTime.Now.AddYears(-2),
                FechaFin = DateTime.Now
            }
        },
                ReferenciasPersonales = new List<ReferenciaPersonalDTO>
        {
            new ReferenciaPersonalDTO
            {
                Nombre = "Carlos Garcia",
                Telefono = "987654321",
                Relacion = "Ex-jefe"
            }
        },
                Idiomas = new List<IdiomaDTO>
        {
            new IdiomaDTO
            {
                NombreIdioma = "Inglés",
                Nivel = "Intermedio"
            }
        }
            };

            // Act
            var result = controller.UpdateHojaDeVida(1, hojaDeVidaDto); // Llama al método del controlador

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            var messageProperty = response.GetType().GetProperty("message").GetValue(response, null);

            // Verifica que el mensaje sea el esperado
            Assert.Equal("La Hoja de Vida ha sido actualizada con éxito.", messageProperty);

            // Verifica que la hoja de vida se haya actualizado en el contexto
            var updatedHojaDeVida = context.HojaDeVida.Include(h => h.FormacionesAcademicas)
                                                        .Include(h => h.ExperienciasProfesionales)
                                                        .Include(h => h.ReferenciasPersonales)
                                                        .Include(h => h.Idiomas)
                                                        .FirstOrDefault(h => h.Id == 1);

            Assert.NotNull(updatedHojaDeVida); // Verifica que no sea nula
            Assert.Equal("Juan Perez Actualizado", updatedHojaDeVida.NombreCompleto); // Verifica que el nombre sea correcto
        }
    }
}
