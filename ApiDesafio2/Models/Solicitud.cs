using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDesafio2.Models
{
    public class Solicitud
    {
        [Key]
        public int SolicitudId { get; set; }

        [ForeignKey("Usuarios")]
        public string UsuarioSolicitanteId { get; set; }

        [ForeignKey("OfertasEmpleo")]
        public int OfertaEmpleoId { get; set; }

        [ForeignKey("HojaDeVida")]
        public int HojaDeVidaId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaPublicacion { get; set; }
    }
}
