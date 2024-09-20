using ApiDesafio2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDesafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly ProyectoDbContext dbContext;

        public SolicitudesController(ProyectoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("CrearSolicitud")]
        public IActionResult CrearSolicitud(SolicitudDto solicitudDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.Solicitudes.Add(new Solicitud
            {
                UsuarioSolicitanteId = solicitudDto.UsuarioSolicitanteId,
                OfertaEmpleoId = solicitudDto.OfertaEmpleoId,
                HojaDeVidaId = solicitudDto.HojaDeVidaId,
                FechaPublicacion = solicitudDto.FechaPublicacion
            });
            dbContext.SaveChanges();
            return Ok(new
            {
                message = "La solicitud se guardo correctamente."
            });
        }

        [HttpGet]
        [Route("GetSolicitudes")]
        public IActionResult GetSolicitudes()
        {
            var solicitudes = dbContext.Solicitudes
                           .Select(o => new
                           {
                               o.UsuarioSolicitanteId,
                               o.OfertaEmpleoId,
                               o.HojaDeVidaId,
                               o.FechaPublicacion
                           }).ToList();

            return Ok(new
            {
                message = "Solicitudes encontradas.",
                solicitudes = solicitudes
            });
        }
    }
}
