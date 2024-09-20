using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiDesafio2.Models
{
    public class SolicitudDto
    {
        public string UsuarioSolicitanteId { get; set; }

        public int OfertaEmpleoId { get; set; }

        public int HojaDeVidaId { get; set; }

        public DateTime FechaPublicacion { get; set; }
    }
}
