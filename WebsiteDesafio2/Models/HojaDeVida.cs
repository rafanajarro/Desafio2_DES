using System.ComponentModel.DataAnnotations;

namespace WebsiteDesafio2.Models
{
    public class HojaDeVida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [StringLength(100)]
        public string usuario { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        public ICollection<FormacionAcademica> FormacionesAcademicas { get; set; }
        public ICollection<ExperienciaProfesional> ExperienciasProfesionales { get; set; }
        public ICollection<ReferenciaPersonal> ReferenciasPersonales { get; set; }
        public ICollection<Idioma> Idiomas { get; set; }
    }
}
