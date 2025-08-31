using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TemuDB.API.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Verwende SQLite für Design-Time (keine MySQL-Verbindung erforderlich)
            optionsBuilder.UseSqlite("Data Source=temudb.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
