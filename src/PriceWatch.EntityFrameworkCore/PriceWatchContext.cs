using Microsoft.EntityFrameworkCore;
using PriceWatch.EntityFrameworkCore.Entities;

namespace PriceWatch.EntityFrameworkCore;

public class PriceWatchContext : DbContext
{
  public PriceWatchContext(DbContextOptions<PriceWatchContext> options) : base(options)
  {
  }

  internal DbSet<PriceHistoryEntity> PriceHistory { get; private set; }
  internal DbSet<ProductEntity> Products { get; private set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
