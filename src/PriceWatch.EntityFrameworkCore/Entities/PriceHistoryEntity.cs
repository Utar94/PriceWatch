using Logitar;
using PriceWatch.Domain.Products;

namespace PriceWatch.EntityFrameworkCore.Entities;

internal class PriceHistoryEntity
{
  public int PriceHistoryId { get; private set; }

  public ProductEntity? Product { get; private set; }
  public int ProductId { get; private set; }

  public DateTime Timestamp { get; private set; }
  public decimal Price { get; private set; }

  public PriceHistoryEntity(ProductEntity product, Product.PriceChanged @event)
  {
    Product = product;
    ProductId = product.ProductId;

    Timestamp = @event.OccurredOn.AsUniversalTime();
    Price = @event.Price;
  }

  private PriceHistoryEntity()
  {
  }
}
