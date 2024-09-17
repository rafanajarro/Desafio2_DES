using System.ComponentModel.DataAnnotations;

namespace SitioWebDesafio2.Models
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100)]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(8)]
        [Phone]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(50)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El rol de usuario es obligatorio.")]
        [StringLength(20)]
        public string RolUsuario { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaNacimiento { get; set; }
    }
}
