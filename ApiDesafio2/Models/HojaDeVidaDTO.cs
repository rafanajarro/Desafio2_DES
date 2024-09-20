using System.ComponentModel.DataAnnotations;

namespace ApiDesafio2.Models
{
    public class HojaDeVidaDTO
    {
        public string NombreCompleto { get; set; }

        public string usuario { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public List<FormacionAcademicaDTO> FormacionesAcademicas { get; set; }
        public List<ExperienciaProfesionalDTO> ExperienciasProfesionales { get; set; }
        public List<ReferenciaPersonalDTO> ReferenciasPersonales { get; set; }
        public List<IdiomaDTO> Idiomas { get; set; }


    }
}
