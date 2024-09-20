using Microsoft.EntityFrameworkCore;

namespace WebsiteDesafio2.Models
{
    public class ProyectoDbContext : DbContext
    {
        public ProyectoDbContext(DbContextOptions<ProyectoDbContext> options) : base(options)
        {
        }

        //public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<OfertaEmpleo> OfertasEmpleo { get; set; }

        public DbSet<HojaDeVida> HojaDeVida { get; set; }
        public DbSet<FormacionAcademica> FormacionesAcademicas { get; set; }
        public DbSet<ExperienciaProfesional> ExperienciasProfesionales { get; set; }
        public DbSet<ReferenciaPersonal> ReferenciasPersonales { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }

    }
}
