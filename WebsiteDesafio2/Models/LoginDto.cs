using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(10)]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]        
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
