using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class ReferenciaPersonal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        public string Relacion { get; set; }

        [ForeignKey("HojaDeVida")]
        public int HojaDeVidaId { get; set; }
        public HojaDeVida HojaDeVida { get; set; }
    }
}
