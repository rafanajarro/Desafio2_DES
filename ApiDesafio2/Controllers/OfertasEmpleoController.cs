using ApiDesafio2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiDesafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasEmpleoController : ControllerBase
    {
        private readonly ProyectoDbContext dbContext;

        public OfertasEmpleoController(ProyectoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("CrearOferta")]
        public IActionResult CrearOferta(OfertaEmpleo ofertaEmpleo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                dbContext.OfertasEmpleo.Add(new OfertaEmpleo
                {
                    OfertaId = ofertaEmpleo.OfertaId,
                    NombreOferta = ofertaEmpleo.NombreOferta,
                    DescripcionOferta = ofertaEmpleo.DescripcionOferta,
                    Requisitos = ofertaEmpleo.Requisitos,
                    Salario = ofertaEmpleo.Salario,
                    FechaPublicacion = DateTime.Now,
                    Contacto = ofertaEmpleo.Contacto,
                    UsuarioId = ofertaEmpleo.UsuarioId
                });
                dbContext.SaveChanges();
                return Ok(new
                {
                    message = "La oferta ha sido creada con exito."
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

        [HttpGet]
        [Route("GetMisOfertas")]
        public IActionResult GetMisOfertas(string codigoUsuario)
        {
            var usuario = dbContext.OfertasEmpleo.FirstOrDefault(x => x.UsuarioId == codigoUsuario);
            if (usuario != null)
            {
                var ofertas = dbContext.OfertasEmpleo
                               .Where(o => o.UsuarioId == codigoUsuario)
                               .Select(o => new
                               {
                                   o.OfertaId,
                                   o.NombreOferta,
                                   o.DescripcionOferta,
                                   o.FechaPublicacion,
                                   o.Salario,
                                   o.Requisitos,
                                   o.Contacto
                               }).ToList();

                return Ok(new
                {
                    message = "Ofertas encontradas.",
                    ofertas = ofertas
                });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("GetOfertas")]
        public IActionResult GetOfertas()
        {
            var ofertas = dbContext.OfertasEmpleo
                           .Select(o => new
                           {
                               o.NombreOferta,
                               o.DescripcionOferta,
                               o.FechaPublicacion,
                               o.Salario,
                               o.Requisitos,
                               o.Contacto
                           }).ToList();

            return Ok(new
            {
                message = "Ofertas encontradas.",
                ofertas = ofertas
            });
        }

        [HttpPut]
        [Route("UpdateOferta")]
        public IActionResult UpdateOferta(int ofertaId, OfertaEmpleo ofertaActualizada)
        {
            var oferta = dbContext.OfertasEmpleo.FirstOrDefault(o => o.OfertaId == ofertaId);

            if (oferta == null)
            {
                return NotFound(new { message = "Oferta no encontrada." });
            }

            // Actualizamos los campos de la oferta
            oferta.NombreOferta = ofertaActualizada.NombreOferta;
            oferta.DescripcionOferta = ofertaActualizada.DescripcionOferta;
            oferta.Requisitos = ofertaActualizada.Requisitos;
            oferta.FechaPublicacion = ofertaActualizada.FechaPublicacion;
            oferta.Salario = ofertaActualizada.Salario;
            oferta.Contacto = ofertaActualizada.Contacto;

            // Guardar cambios en la base de datos
            dbContext.SaveChanges();

            return Ok(new { message = "Oferta actualizada correctamente." });
        }

        [HttpDelete]
        [Route("EliminarOferta")]
        public IActionResult EliminarOferta(int ofertaId)
        {
            var oferta = dbContext.OfertasEmpleo.FirstOrDefault(o => o.OfertaId == ofertaId);

            if (oferta == null)
            {
                return NotFound(new { message = "Oferta no encontrada." });
            }

            // Eliminar la oferta
            dbContext.OfertasEmpleo.Remove(oferta);

            // Guardar cambios en la base de datos
            dbContext.SaveChanges();

            return Ok(new { message = "Oferta eliminada correctamente." });
        }
    }
}
