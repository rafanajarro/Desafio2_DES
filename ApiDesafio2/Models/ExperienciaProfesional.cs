using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiDesafio2.Models
{
    public class ExperienciaProfesional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Empresa { get; set; }

        [Required]
        [StringLength(100)]
        public string Cargo { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [ForeignKey("HojaDeVida")]
        public int HojaDeVidaId { get; set; }
        public HojaDeVida HojaDeVida { get; set; }
    }
}
