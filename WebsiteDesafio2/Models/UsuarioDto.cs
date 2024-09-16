using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class UsuarioDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(8)]
        [Phone]
        public string Telefono { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(20)]
        public string RolUsuario { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaNacimiento { get; set; }
    }
}
