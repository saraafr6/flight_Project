using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrustructure.Data
{
    public class FlyDbContext : DbContext
    {
        public FlyDbContext(DbContextOptions<FlyDbContext> options)
            : base(options)
        {
        }
        public FlyDbContext()
            : base(new DbContextOptionsBuilder<FlyDbContext>()
                    .UseSqlServer(Configuration().GetConnectionString("AppConectionString"), sqlServerOptions => sqlServerOptions.CommandTimeout(20*60)).Options)
        {
        }

        public static IConfiguration Configuration()
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
            .Build();

            return config;
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .Property(i => i.Id)
        //        .ValueGeneratedOnAdd();
        //    //modelBuilder.Entity<RelatedSubjects>()
        //    //       .HasRequired(m => m.)
        //    //       .WithMany(t => t.HomeMatches)
        //    //       .HasForeignKey(m => m.HomeTeamId)
        //    //       .WillCascadeOnDelete(false);

        //    //modelBuilder.Entity<RelatedSubjects>()
        //    //            .HasRequired(m => m.GuestTeam)
        //    //            .WithMany(t => t.AwayMatches)
        //    //            .HasForeignKey(m => m.GuestTeamId)
        //    //            .WillCascadeOnDelete(false);
        //}

        //public DbSet<User> User { get; set; }
    }
}
