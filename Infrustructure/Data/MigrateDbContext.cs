using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrustructure.Data
{
    public class MigrateDbContext : FlyDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = Configuration().GetConnectionString("AppConectionString");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
