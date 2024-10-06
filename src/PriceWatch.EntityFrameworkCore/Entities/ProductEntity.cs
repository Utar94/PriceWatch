using PriceWatch.Domain.Products;

namespace PriceWatch.EntityFrameworkCore.Entities;

internal class ProductEntity : AggregateEntity
{
  public int ProductId { get; private set; }
  public Guid Id { get; private set; }

  public string DisplayName { get; } = string.Empty;

  public Supplier Supplier { get; }
  public string Url { get; } = string.Empty;

  public bool IsBeingWatched { get; private set; }
  public decimal? CurrentPrice { get; private set; }

  public List<PriceHistoryEntity> PriceHistory { get; private set; } = [];

  public ProductEntity(Product.CreatedEvent @event) : base(@event)
  {
    Id = @event.AggregateId.ToGuid();

    DisplayName = @event.DisplayName.Value;

    Supplier = @event.Supplier;
    Url = @event.Url.ToString();
  }

  private ProductEntity() : base()
  {
  }

  public void SetPrice(Product.PriceChanged @event)
  {
    Update(@event);

    CurrentPrice = @event.Price;

    PriceHistory.Add(new PriceHistoryEntity(this, @event));
  }

  public void Unwatch(Product.UnwatchedEvent @event)
  {
    Update(@event);

    IsBeingWatched = false;
  }

  public void Watch(Product.WatchedEvent @event)
  {
    Update(@event);

    IsBeingWatched = true;
  }
}
