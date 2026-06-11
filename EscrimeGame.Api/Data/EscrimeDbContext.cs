using EscrimeGame;
using Microsoft.EntityFrameworkCore;

namespace EscrimeGame.Api.Data;

public class EscrimeDbContext : DbContext
{
    public EscrimeDbContext(DbContextOptions<EscrimeDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<MatchResult> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>()
            .HasMany(p => p.Matches)
            .WithOne()
            .HasForeignKey(m => m.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
