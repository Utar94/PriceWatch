using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceWatch.EntityFrameworkCore.Entities;

namespace PriceWatch.EntityFrameworkCore.Configurations;

internal class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistoryEntity>
{
  public void Configure(EntityTypeBuilder<PriceHistoryEntity> builder)
  {
    builder.ToTable(nameof(PriceWatchContext.PriceHistory));
    builder.HasKey(x => x.PriceHistoryId);

    builder.HasIndex(x => x.Timestamp);
    builder.HasIndex(x => x.Price);

    builder.Property(x => x.Price).HasColumnType("money");

    builder.HasOne(x => x.Product).WithMany(x => x.PriceHistory)
      .HasPrincipalKey(x => x.ProductId).HasForeignKey(x => x.ProductId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
