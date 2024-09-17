using Microsoft.EntityFrameworkCore;
using SitioWebDesafio2.Models;

namespace SitioWebDesafio2.Models
{
    public class ProyectoDbContext : DbContext
    {
        public ProyectoDbContext(DbContextOptions<ProyectoDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<OfertaEmpleo> OfertasEmpleo { get; set; }
    }
}
