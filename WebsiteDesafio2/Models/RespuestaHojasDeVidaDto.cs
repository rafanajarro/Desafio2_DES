namespace WebsiteDesafio2.Models
{
    public class RespuestaHojasDeVidaDto
    {
        public string message { get; set; }
        public List<HojaDeVidaDto> hojas { get; set; }
    }

    public class HojaDeVidaDto
    {
        public int Id { get; set; }
        public string usuario { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public List<FormacionAcademicaDto> FormacionesAcademicas { get; set; }
        public List<ExperienciaProfesionalDto> ExperienciasProfesionales { get; set; }
        public List<ReferenciaPersonalDto> ReferenciasPersonales { get; set; }
        public List<IdiomaDto> Idiomas { get; set; }
    }

    public class FormacionAcademicaDto
    {
        public string TituloObtenido { get; set; }
        public string Institucion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class ExperienciaProfesionalDto

    {
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string descripcion { get; set; }
    }

    public class ReferenciaPersonalDto
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Relacion { get; set; }
    }

    public class IdiomaDto
    {
        public string NombreIdioma { get; set; }
        public string Nivel { get; set; }
    }
}
