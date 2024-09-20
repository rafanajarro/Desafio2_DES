namespace ApiDesafio2.Models
{
    public class FormacionAcademicaDTO
    {
        public int Id { get; set; }
        public string Institucion { get; set; }
        public string TituloObtenido { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
