﻿using Microsoft.AspNetCore.Mvc;
using ApiDesafio2.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDesafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HojaDeVidaController : Controller
    {
        private readonly ProyectoDbContext dbContext;

        public HojaDeVidaController(ProyectoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        [Route("GetMisHojasDeVida")]
        public IActionResult GetMisHojasDeVida(string codigoUsuario)
        {
            var usuario = dbContext.HojaDeVida.FirstOrDefault(x => x.usuario == codigoUsuario);
            if (usuario != null)
            {
          
                
           var hojas = dbContext.HojaDeVida
         .Where(o => o.usuario == codigoUsuario)
         .Select(o => new
         {
             o.Id,
             o.usuario,
             o.NombreCompleto,
             o.FechaNacimiento,
             FormacionesAcademicas = o.FormacionesAcademicas.Select(f => new
             {
                 f.TituloObtenido,
                 f.Institucion,
                 f.FechaInicio,
                 f.FechaFin
             }).ToList(),
             ExperienciasProfesionales = o.ExperienciasProfesionales.Select(e => new
             {
                 e.Empresa,
                 e.Cargo,
                 e.FechaInicio,
                 e.FechaFin
             }).ToList(),
             ReferenciasPersonales = o.ReferenciasPersonales.Select(r => new
             {
                 r.Nombre,
                 r.Telefono,
                 r.Relacion
             }).ToList(),
             Idiomas = o.Idiomas.Select(i => new
             {
                 i.NombreIdioma,
                 i.Nivel
             }).ToList()
         }).ToList();


                return Ok(new
                {
                    message = "Hojas Encontradas.",
                    hojas = hojas
                });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("GetHojaDeVida")]
        public IActionResult GetHojasDeVida(int id)
        {
  
            var registro = dbContext.HojaDeVida.FirstOrDefault(x => x.Id == id);

            if (registro != null)
            {


                var hojas = dbContext.HojaDeVida
              .Where(o => o.Id == id)
              .Select(o => new
              {
                  o.Id,
                  o.usuario,
                  o.NombreCompleto,
                  o.FechaNacimiento,
                  FormacionesAcademicas = o.FormacionesAcademicas.Select(f => new
                  {
                      f.TituloObtenido,
                      f.Institucion,
                      f.FechaInicio,
                      f.FechaFin
                  }).ToList(),
                  ExperienciasProfesionales = o.ExperienciasProfesionales.Select(e => new
                  {
                      e.Empresa,
                      e.Cargo,
                      e.FechaInicio,
                      e.FechaFin
                  }).ToList(),
                  ReferenciasPersonales = o.ReferenciasPersonales.Select(r => new
                  {
                      r.Nombre,
                      r.Telefono,
                      r.Relacion
                  }).ToList(),
                  Idiomas = o.Idiomas.Select(i => new
                  {
                      i.NombreIdioma,
                      i.Nivel
                  }).ToList()
              }).FirstOrDefault();


                return Ok(new
                {
                    message = "Hoja Encontradas.",
                    hojas = hojas
                });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public IActionResult agregarHojaDeVida([FromBody] HojaDeVidaDTO hojaDeVidaDto)
        {
            // Validación de datos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapear el request body a la entidad HojaDeVida
            var hojaDeVida = new HojaDeVida
            {
                NombreCompleto = hojaDeVidaDto.NombreCompleto,
                FechaNacimiento = hojaDeVidaDto.FechaNacimiento,
                usuario = hojaDeVidaDto.usuario,

                FormacionesAcademicas = new List<FormacionAcademica>(),
                ExperienciasProfesionales = new List<ExperienciaProfesional>(),
                ReferenciasPersonales = new List<ReferenciaPersonal>(),
                Idiomas = new List<Idioma>()
            };

            foreach (var formacion in hojaDeVidaDto.FormacionesAcademicas)
            {
                hojaDeVida.FormacionesAcademicas.Add(new FormacionAcademica { 
                    FechaFin = formacion.FechaFin,
                    FechaInicio = formacion.FechaInicio, 
                    Institucion = formacion.Institucion,
                    TituloObtenido = formacion.TituloObtenido
                    
                });
            }

            foreach(var empleo in hojaDeVidaDto.ExperienciasProfesionales)
            {
                hojaDeVida.ExperienciasProfesionales.Add(new ExperienciaProfesional{ Cargo = empleo.Cargo, Descripcion=empleo.Descripcion, Empresa=empleo.Empresa, FechaFin=empleo.FechaFin, FechaInicio=empleo.FechaInicio});
            }

            foreach (var referenciaDto in hojaDeVidaDto.ReferenciasPersonales)
            {
                hojaDeVida.ReferenciasPersonales.Add(new ReferenciaPersonal
                {
                    Nombre = referenciaDto.Nombre,
                    Telefono = referenciaDto.Telefono,
                    Relacion = referenciaDto.Relacion
                });

            }

            foreach (var idioma in hojaDeVidaDto.Idiomas)
            {
                hojaDeVida.Idiomas.Add(new Idioma
                {
                    NombreIdioma = idioma.NombreIdioma,
                    Nivel = idioma.Nivel
                });

            }

            try
            {
                // Guardar en la base de datos
                dbContext.HojaDeVida.Add(hojaDeVida);
                dbContext.SaveChanges();

                return Ok(new
                {
                    message = "La Hoja de Vida ha sido creada con exito."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(new
                {
                    message = "No se pudo crear la oferta de empleo."
                });
            }

           


        }
    }
}
