namespace WebsiteDesafio2.Models
{
    public class OfertaEmpleoDto
    {
        public int OfertaId { get; set; }
        public string NombreOferta { get; set; }
        public string DescripcionOferta { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public decimal Salario { get; set; }
        public string Requisitos { get; set; }
        public string Contacto { get; set; }
    }
}
