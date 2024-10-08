﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebsiteDesafio2.Models;

namespace WebsiteDesafio2.Controllers
{
    public class OfertaEmpleosController : Controller
    {
        private readonly ProyectoDbContext _context;
        private readonly ApiService _apiService;
        private readonly string urlApi = "https://localhost:7042/api/OfertasEmpleo";
        private readonly string urlApiSoli = "https://localhost:7042/api/Solicitudes";

        public OfertaEmpleosController(ApiService apiService, ProyectoDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        public ActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("NombreUsuario");
            HttpContext.Session.Remove("CorreoElectronico");
            HttpContext.Session.Remove("RolUsuario");
            HttpContext.Session.Remove("Nombre");
            HttpContext.Session.Remove("Apellidos");
            TempData["Message"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Index", "Auth");
        }

        // GET: OfertaEmpleos
        public async Task<IActionResult> Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var rolUsuario = HttpContext.Session.GetString("RolUsuario");
            if (!string.IsNullOrEmpty(nombreUsuario) && !string.IsNullOrEmpty(rolUsuario))
            {
                if (rolUsuario.Equals("Ofertador"))
                {
                    var respuestaPost = await _apiService.ObtenerDatosDeApi(urlApi + "/GetMisOfertas?codigoUsuario=" + nombreUsuario);

                    if (!string.IsNullOrEmpty(respuestaPost))
                    {
                        var respuesta = JsonConvert.DeserializeObject<RespuestaOfertaDto>(respuestaPost);

                        if (respuesta != null && respuesta.message == "Ofertas encontradas.")
                        {
                            // Pasamos la lista de ofertas directamente a la vista
                            return View("Index", respuesta.ofertas);
                        }
                        else
                        {
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    // SOLICITANTE
                    var respuestaPost = await _apiService.ObtenerDatosDeApi(urlApi + "/GetOfertas");

                    if (!string.IsNullOrEmpty(respuestaPost))
                    {
                        var respuesta = JsonConvert.DeserializeObject<RespuestaOfertaDto>(respuestaPost);

                        if (respuesta != null && respuesta.message == "Ofertas encontradas.")
                        {
                            // Pasamos la lista de ofertas directamente a la vista
                            return View("Index", respuesta.ofertas);
                        }
                        else
                        {
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            else
            {
                TempData["Error"] = "No se encontraron datos en la sesión.";
                return RedirectToAction("Index", "Auth");
            }
        }

        // GET: OfertaEmpleos/Details/5
        public async Task<IActionResult> VerSolicitudes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var solicitudOferta = await _context.Solicitudes
                .FirstOrDefaultAsync(m => m.OfertaEmpleoId == id);

            if (solicitudOferta == null)
            {
                TempData["Message"] = "No hay solicitudes para esta oferta.";
                return RedirectToAction("Index");
            }
            
            var dbUsuario = _context.Usuarios.FirstOrDefault(x => x.NombreUsuario == solicitudOferta.UsuarioSolicitanteId);
            // Si no hay experiencias, formaciones, idiomas o referencias, se devuelve una lista vacía.
            var dbExperiencias = _context.ExperienciasProfesionales
                .Where(x => x.HojaDeVidaId == solicitudOferta.HojaDeVidaId).ToList() ?? new List<ExperienciaProfesional>();

            var dbFormaciones = _context.FormacionesAcademicas
                .Where(x => x.HojaDeVidaId == solicitudOferta.HojaDeVidaId).ToList() ?? new List<FormacionAcademica>();

            var dbIdiomas = _context.Idiomas
                .Where(x => x.HojaDeVidaId == solicitudOferta.HojaDeVidaId).ToList() ?? new List<Idioma>();

            var dbReferencias = _context.ReferenciasPersonales
                .Where(x => x.HojaDeVidaId == solicitudOferta.HojaDeVidaId).ToList() ?? new List<ReferenciaPersonal>();

            var datos = new ResponseSolicitudDto
            {
                Usuario = dbUsuario,
                ExperienciaProfesionales = dbExperiencias,
                FormacionAcademicas = dbFormaciones,
                Idioma = dbIdiomas,
                ReferenciaPersonales = dbReferencias
            };

            return View(datos);
        }

        public async Task<IActionResult> Aplicar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbSolicitud = _context.Solicitudes.FirstOrDefault(x => x.OfertaEmpleoId == id);
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var dbHoja = _context.HojaDeVida.FirstOrDefault(x => x.usuario == nombreUsuario);

            Console.WriteLine("dbSolicitud " + dbSolicitud);
            Console.WriteLine("nombreUsuario " + nombreUsuario);
            Console.WriteLine("USUARIO HOJA DE VIDA " + dbHoja);

            bool existeRegistro = _context.Solicitudes
            .Any(e => e.OfertaEmpleoId == id && e.UsuarioSolicitanteId == nombreUsuario);
            Console.WriteLine(existeRegistro);

            if (existeRegistro)
            {
                TempData["Error"] = "Error. Solamente se puede aplicar una vez por cada oferta de empleo.";
                return RedirectToAction("Index");
            }
            else
            {
                if (dbHoja == null)
                {
                    TempData["Error"] = "Debe crear una hoja de vida antes de aplicar a una oferta de empleo.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var datos = new
                    {
                        UsuarioSolicitanteId = nombreUsuario,
                        OfertaEmpleoId = id,
                        HojaDeVidaId = dbHoja.Id,
                        FechaPublicacion = DateTime.Now
                    };

                    var response = await _apiService.EnviarDatosALaApi(urlApiSoli + "/CrearSolicitud", datos);

                    if (response != null)
                    {
                        TempData["Message"] = "Operación realizada con éxito";
                        return RedirectToAction("Index");
                    }

                    TempData["Error"] = "Error al crear la solicitud.";
                    return RedirectToAction("Index");
                }

            }
        }

        public IActionResult HojaDeVida()
        {
            return RedirectToAction("Index", "HojaDeVida");
        }

        // GET: OfertaEmpleos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfertaEmpleos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfertaId,NombreOferta,DescripcionOferta,Requisitos,Salario,FechaPublicacion,Contacto,UsuarioId")] OfertaEmpleo ofertaEmpleo)
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var datos = new
            {
                nombreOferta = ofertaEmpleo.NombreOferta,
                descripcionOferta = ofertaEmpleo.DescripcionOferta,
                requisitos = ofertaEmpleo.Requisitos,
                contacto = ofertaEmpleo.Contacto,
                salario = ofertaEmpleo.Salario,
                usuarioId = nombreUsuario
            };

            var response = await _apiService.EnviarDatosALaApi(urlApi + "/CrearOferta", datos);

            if (response != null)
            {
                TempData["Message"] = "Oferta de empleo creada con exito.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Error al crear la oferta.";
            return View(ofertaEmpleo);
        }

        // GET: OfertaEmpleos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Console.WriteLine(id);
            var ofertaEmpleo = await _context.OfertasEmpleo.FindAsync(id);
            if (ofertaEmpleo == null)
            {
                return NotFound();
            }
            return View(ofertaEmpleo);
        }

        // POST: OfertaEmpleos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OfertaId,NombreOferta,DescripcionOferta,Requisitos,Salario,FechaPublicacion,Contacto,UsuarioId")] OfertaEmpleo ofertaEmpleo)
        {
            if (id != ofertaEmpleo.OfertaId)
            {
                return NotFound();
            }

            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var datos = new
            {
                nombreOferta = ofertaEmpleo.NombreOferta,
                descripcionOferta = ofertaEmpleo.DescripcionOferta,
                requisitos = ofertaEmpleo.Requisitos,
                salario = ofertaEmpleo.Salario,
                fechaPublicacion = ofertaEmpleo.FechaPublicacion,
                contacto = ofertaEmpleo.Contacto,
                usuarioId = nombreUsuario
            };

            try
            {
                var response = await _apiService.ActualizarDatosApi(urlApi + "/UpdateOferta?ofertaId=" + id, datos);

                if (response != null)
                {
                    TempData["Message"] = "Oferta de empleo actualizada con exito.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error al actualizar la oferta.";
                return View(ofertaEmpleo);
            }
            catch (Exception ex)
            {
                return View(ofertaEmpleo);
            }
        }

        // GET: OfertaEmpleos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofertaEmpleo = await _context.OfertasEmpleo
                .FirstOrDefaultAsync(m => m.OfertaId == id);
            if (ofertaEmpleo == null)
            {
                return NotFound();
            }

            return View(ofertaEmpleo);
        }

        // POST: OfertaEmpleos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiService.EliminarDatosApi(urlApi + "/EliminarOferta?ofertaId=" + id);
            TempData["Message"] = "Oferta de empleo eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool OfertaEmpleoExists(int id)
        {
            return _context.OfertasEmpleo.Any(e => e.OfertaId == id);
        }

        public ActionResult Regresar()
        {
            return RedirectToAction("Index", "OfertaEmpleos");
        }
    }
}
