
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System.Net;
using WebsiteDesafio2.Models;

namespace WebsiteDesafio2.Controllers
{
    public class HojaDeVidaController : Controller
    {

        private readonly ProyectoDbContext _context;
        private readonly ApiService _apiService;
        private readonly string urlApi = "https://localhost:7042/api";

        public HojaDeVidaController(ApiService apiService, ProyectoDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }       

        public ActionResult Regresar()
        {
            return RedirectToAction("Index", "OfertaEmpleos");
        }

        public async Task<IActionResult> Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                var respuestaPost = await _apiService.ObtenerDatosDeApi(urlApi + "/HojaDeVida/GetMisHojasDeVida?codigoUsuario=" + nombreUsuario);

                if (!string.IsNullOrEmpty(respuestaPost))
                {

                    var respuesta = JsonConvert.DeserializeObject<RespuestaHojasDeVidaDto>(respuestaPost);
                    Console.WriteLine(respuesta);

                    if (respuesta != null && respuesta.message == "Hojas Encontradas.")
                    {
                        return View("Index", respuesta.hojas);
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
                TempData["Error"] = "No se encontraron datos en la sesión.";
                return RedirectToAction("Index", "Auth");
            }
        }

        public ActionResult Create()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var hojaVida = _context.HojaDeVida.FirstOrDefault(x => x.usuario == nombreUsuario);

            if (hojaVida == null)
            {
                return View();
            }
            else
            {
                TempData["Error"] = "Ya existe una hoja de vida. Puedes modificar o eliminar la existente.";
                return RedirectToAction("Index", "HojaDeVida");
            }
        }


        // GET: https://localhost:7042/api/HojaDeVida/GetHojaDeVida?id=5
        public async Task<IActionResult> Edit(int id)
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                var respuestaPost = await _apiService.ObtenerDatosDeApi(urlApi + "/HojaDeVida/GetHojaDeVida?id=" + id);
                if (!string.IsNullOrEmpty(respuestaPost))
                {
                    try
                    {
                        var respuesta = JsonConvert.DeserializeObject<RespuestaHojaDeVidaDto>(respuestaPost);
                        if (respuesta.message == "Hoja Encontradas.")
                        {
                            return View(respuesta.hoja);
                        }
                        else
                        {
                            TempData["Error"] = "No se encontraron hojas.";
                            return RedirectToAction("Index", "HojaDeVida");
                        }
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "No se encontraron hojas.";
                    return RedirectToAction("Index", "HojaDeVida");
                }
            }
            else
            {
                TempData["Error"] = "No se encontraron datos en la sesión.";
                return RedirectToAction("Index", "Auth");
            }
        }

        // POST: OfertaEmpleos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiService.EliminarDatosApi(urlApi + "/HojaDeVida/EliminarHoja?id=" + id);
            TempData["Message"] = "Hoja de vida eliminada con exito.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCompleto,FechaNacimiento,FormacionesAcademicas,ExperienciasProfesionales,ReferenciasPersonales,Idiomas")] HojaDeVida hojaDeVida)
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            var datos = new
            {
                nombreCompleto = hojaDeVida.NombreCompleto,
                usuario = nombreUsuario,
                fechaNacimiento = hojaDeVida.FechaNacimiento.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                formacionesAcademicas = hojaDeVida.FormacionesAcademicas.Select(f => new
                {
                    id = f.Id,
                    institucion = f.Institucion,
                    tituloObtenido = f.TituloObtenido,
                    fechaInicio = f.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    fechaFin = f.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                }),
                experienciasProfesionales = hojaDeVida.ExperienciasProfesionales.Select(e => new
                {
                    empresa = e.Empresa,
                    cargo = e.Cargo,
                    fechaInicio = e.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    fechaFin = e.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    descripcion = e.Descripcion
                }),
                referenciasPersonales = hojaDeVida.ReferenciasPersonales.Select(r => new
                {
                    nombre = r.Nombre,
                    telefono = r.Telefono,
                    relacion = r.Relacion
                }),
                idiomas = hojaDeVida.Idiomas.Select(i => new
                {
                    nombreIdioma = i.NombreIdioma,
                    nivel = i.Nivel
                })
            };

            var response = await _apiService.EnviarDatosALaApi(urlApi + "/HojaDeVida", datos);

            if (response != null)
            {
                TempData["Message"] = "Hoja de vida creada con exito.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error. No se pudo crear la hoja de vida.";
            return RedirectToAction("Index", "HojaDeVida");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarHoja(int id, [Bind("NombreCompleto,FechaNacimiento,FormacionesAcademicas,ExperienciasProfesionales,ReferenciasPersonales,Idiomas")] HojaDeVida hojaDeVida)
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            Console.WriteLine(nombreUsuario + "Modificar");
            // Verifica si el estado del modelo es inválido

            // Preparar los datos
            var datos = new
            {
                nombreCompleto = hojaDeVida.NombreCompleto,
                usuario = nombreUsuario,
                fechaNacimiento = hojaDeVida.FechaNacimiento.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                formacionesAcademicas = hojaDeVida.FormacionesAcademicas.Select(f => new
                {
                    id = f.Id,
                    institucion = f.Institucion,
                    tituloObtenido = f.TituloObtenido,
                    fechaInicio = f.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    fechaFin = f.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                }),
                experienciasProfesionales = hojaDeVida.ExperienciasProfesionales.Select(e => new
                {
                    empresa = e.Empresa,
                    cargo = e.Cargo,
                    fechaInicio = e.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    fechaFin = e.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    descripcion = "Text"
                }),
                referenciasPersonales = hojaDeVida.ReferenciasPersonales.Select(r => new
                {
                    nombre = r.Nombre,
                    telefono = r.Telefono,
                    relacion = r.Relacion
                }),
                idiomas = hojaDeVida.Idiomas.Select(i => new
                {
                    nombreIdioma = i.NombreIdioma,
                    nivel = i.Nivel
                })
            };

            // Convertir los datos a JSON
            var json = JsonConvert.SerializeObject(datos);

            // Llamada al servicio API
            var response = await _apiService.ActualizarDatosApi(urlApi + "/HojaDeVida/UpdateHojaDeVida?id=" + id, datos);

            // Verificar la respuesta de la API
            if (response != null)
            {
                // Redirigir a la lista de hojas de vida después de éxito
                TempData["Message"] = "Hoja de vida actualizada con exito.";
                return RedirectToAction("Index", "HojaDeVida");
            }

            // Si el API falla, redirigir a una página de error o a la lista de hojas de vida
            TempData["Error"] = "Error. No se pudo actualizar la hoja de vida.";
            return RedirectToAction("Index", "HojaDeVida");
        }

    }
}
