using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiDesafio2.Models
{
    public class FormacionAcademica
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Institucion { get; set; }

        [Required]
        [StringLength(100)]
        public string TituloObtenido { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [ForeignKey("HojaDeVida")]
        public int HojaDeVidaId { get; set; }
        public HojaDeVida HojaDeVida { get; set; }
    }
}
