using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using WebsiteDesafio2.Models;

namespace WebsiteDesafio2.Controllers
{
    public class ReportController : Controller
    {
        private readonly ProyectoDbContext _context;

        public ReportController(ProyectoDbContext context)
        {
            _context = context;
        }

        public IActionResult GeneratePdfReport()
        {
            // 1. Realizar el query a la base de datos
            var data = _context.HojaDeVida
                .Select(x => new { x.Id, x.NombreCompleto, x.usuario })
                .ToList();

            // 2. Crear el archivo PDF en memoria
            using (MemoryStream stream = new MemoryStream())
            {
                // Inicializar el documento PDF
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document document = new Document(pdfDoc);

                // Título del documento
                document.Add(new Paragraph("Reporte de Datos"));

                // 3. Agregar los datos del query al PDF
                foreach (var item in data)
                {
                    document.Add(new Paragraph($"ID: {item.Id}"));
                    document.Add(new Paragraph($"Nombre: {item.NombreCompleto}"));
                    document.Add(new Paragraph($"usuario: {item.usuario}"));
                    document.Add(new Paragraph("---------------------------------------------"));
                }

                // 4. Cerrar el documento
                document.Close();

                // Devolver el archivo PDF generado
                return File(stream.ToArray(), "application/pdf", "ReporteHojaDeVida.pdf");
            }
        }
    }
}
