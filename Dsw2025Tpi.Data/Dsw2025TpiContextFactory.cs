using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data
{
    public class Dsw2025TpiContextFactory : IDesignTimeDbContextFactory<Dsw2025TpiContext>
    {
        public Dsw2025TpiContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Dsw2025TpiContext>();
            optionsBuilder.UseSqlServer("Server=(local)\\SQLEXPRESS;Database=Dsw2025TpiDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new Dsw2025TpiContext(optionsBuilder.Options);
        }
        
        }

    }

