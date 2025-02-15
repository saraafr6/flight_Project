using Microsoft.EntityFrameworkCore;

namespace Fly.Data
{
    public class FlyDbContext : DbContext
    {
        public FlyDbContext(DbContextOptions<FlyDbContext> options)
            : base(options)
        {
        }
        public FlyDbContext() : base(new DbContextOptionsBuilder<FlyDbContext>()
                                         .UseSqlServer(DefaultAppEnvionmentsProvider.GetConfiguration().GetConnectionString("AppConnectionString"), sqlServerOptions => sqlServerOptions.CommandTimeout(20 * 60)).Options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
            //modelBuilder.Entity<RelatedSubjects>()
            //       .HasRequired(m => m.)
            //       .WithMany(t => t.HomeMatches)
            //       .HasForeignKey(m => m.HomeTeamId)
            //       .WillCascadeOnDelete(false);

            //modelBuilder.Entity<RelatedSubjects>()
            //            .HasRequired(m => m.GuestTeam)
            //            .WithMany(t => t.AwayMatches)
            //            .HasForeignKey(m => m.GuestTeamId)
            //            .WillCascadeOnDelete(false);
        }

        public DbSet<User> User { get; set; }
    }
}
