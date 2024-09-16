using Microsoft.EntityFrameworkCore;

namespace ApiDesafio2.Models
{
    public class ProyectoDbContext : DbContext
    {
        public ProyectoDbContext(DbContextOptions<ProyectoDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
