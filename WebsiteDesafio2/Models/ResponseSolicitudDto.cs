namespace WebsiteDesafio2.Models
{
    public class ResponseSolicitudDto
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public List<ExperienciaProfesional>? ExperienciaProfesionales { get; set; }
        public List<FormacionAcademica>? FormacionAcademicas { get; set; }
        public List<Idioma>? Idioma { get; set; }
        public List<ReferenciaPersonal>? ReferenciaPersonales { get; set; }
    }
}
