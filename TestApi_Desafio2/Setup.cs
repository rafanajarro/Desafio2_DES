using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDesafio2.Models;

using Microsoft.EntityFrameworkCore;

namespace TestApi_Desafio2
{
    public static class Setup
    {
        public static ProyectoDbContext GetInMemoryDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ProyectoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ProyectoDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
