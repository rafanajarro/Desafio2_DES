using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class Usuario
    {
        [Required]
        [StringLength(10)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(20)]
        public string RolUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellidos { get; set; }
    }
}
