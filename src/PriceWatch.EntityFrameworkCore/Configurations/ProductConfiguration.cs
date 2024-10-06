using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PriceWatch.Domain.Products;
using PriceWatch.EntityFrameworkCore.Entities;

namespace PriceWatch.EntityFrameworkCore.Configurations;

internal class ProductConfiguration : AggregateConfiguration<ProductEntity>, IEntityTypeConfiguration<ProductEntity>
{
  private const int UrlMaximumLength = 2048;

  public override void Configure(EntityTypeBuilder<ProductEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PriceWatchContext.Products));
    builder.HasKey(x => x.ProductId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.Supplier);
    builder.HasIndex(x => x.IsBeingWatched);
    builder.HasIndex(x => x.CurrentPrice);

    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.Supplier).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Supplier>());
    builder.Property(x => x.Url).HasMaxLength(UrlMaximumLength);
    builder.Property(x => x.CurrentPrice).HasColumnType("money");
  }
}
