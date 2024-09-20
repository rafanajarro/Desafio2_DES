using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class Idioma
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreIdioma { get; set; }

        [Required]
        [StringLength(50)]
        public string Nivel { get; set; }

        [ForeignKey("HojaDeVida")]
        public int HojaDeVidaId { get; set; }
        public HojaDeVida HojaDeVida { get; set; }
    }
}
