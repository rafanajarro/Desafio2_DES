using System.ComponentModel.DataAnnotations;

namespace ApiDesafio2.Models
{
    public class LoginDto
    {
        [Required]
        [StringLength(10)]
        public string NombreUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
