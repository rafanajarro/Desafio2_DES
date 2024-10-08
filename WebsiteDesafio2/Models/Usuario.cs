using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class Usuario : IdentityUser
    {
        public int UsuarioId { get; set; }

        [StringLength(10)]
        public string NombreUsuario { get; set; }

        [StringLength(8)]
        [Phone]
        public string Telefono { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellidos { get; set; }

        [StringLength(20)]
        public string RolUsuario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaNacimiento { get; set; }
    }
}
