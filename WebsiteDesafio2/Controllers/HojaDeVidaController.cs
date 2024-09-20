using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using WebsiteDesafio2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<IActionResult> Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                var respuestaPost = await _apiService.ObtenerDatosDeApi(urlApi + "/HojaDeVida/GetMisHojasDeVida?codigoUsuario=" + nombreUsuario);

                if (!string.IsNullOrEmpty(respuestaPost))
                {
                    try
                    {
                        var respuesta = JsonConvert.DeserializeObject<RespuestaHojasDeVidaDto>(respuestaPost);
                        Console.WriteLine(respuesta);
                        if (respuesta == null)
                        {
                            ViewBag.Error = "No se encontraron hojas de vida.";
                            return View();
                        }

                        if (respuesta.message == "Hojas Encontradas.")
                        {
                            return View("Index", respuesta.hojas);
                        }
                        else
                        {
                            ViewBag.Error = "No se encontraron Hojas 1.";
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontraron Hojas 2.";
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
            return View();
        }


        // GET: https://localhost:7042/api/HojaDeVida/GetHojaDeVida?id=5
        public async Task<IActionResult> Edit(int id)
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                var respuestaPost = await _apiService.ObtenerDatosDeApi(urlApi + "/HojaDeVida/GetHojaDeVida?id=" + id);
                Console.WriteLine(respuestaPost);
                if (!string.IsNullOrEmpty(respuestaPost))
                {
                    try
                    {
                        var respuesta = JsonConvert.DeserializeObject<RespuestaHojaDeVidaDto>(respuestaPost);
                        if (respuesta.message == "Hojas Encontradas.")
                        {
                            Console.WriteLine(respuesta.hoja);
                            return View(respuesta.hoja);//ENVIA HOJA DE VIDA DTO
                        }
                        else
                        {
                            ViewBag.Error = "No se encontraron hojas de vida.";
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "No hay hojas de vida guardadas.";
                    return View();
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
            return RedirectToAction("Index");
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
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Error al crear la hoja de vida.";
            return View(hojaDeVida);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("NombreCompleto,FechaNacimiento,FormacionesAcademicas,ExperienciasProfesionales,ReferenciasPersonales,Idiomas")] HojaDeVida hojaDeVida)
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            if (!ModelState.IsValid)
            {
                return View(hojaDeVida);
            }

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

            var json = (JsonConvert.SerializeObject(datos));
            var response = await _apiService.ActualizarDatosApi(urlApi + "/HojaDeVida/UpdateHojaDeVida?id=" + id, datos);

            if (response != null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Error al crear la hoja de vida.";
            return View(hojaDeVida);
        }

    }
}
