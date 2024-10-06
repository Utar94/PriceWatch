using Logitar.EventSourcing;
using MediatR;

namespace PriceWatch.Domain.Products;

public class Product : AggregateRoot
{
  public new ProductId Id => new(base.Id);

  private DisplayName? _displayName = null;
  public DisplayName DisplayName => _displayName ?? throw new InvalidOperationException($"The {nameof(DisplayName)} has not been initialized yet.");

  public Supplier Supplier { get; private set; }
  private Uri? _url = null;
  public Uri Url => _url ?? throw new InvalidOperationException($"The {nameof(Url)} has not been initialized yet.");

  public bool IsBeingWatched { get; private set; }
  public decimal? CurrentPrice { get; private set; }

  public Product() : base()
  {
  }

  public Product(DisplayName displayName, Supplier supplier, Uri url, Guid? id = null)
    : base((id.HasValue ? new ProductId(id.Value) : new ProductId()).AggregateId)
  {
    Raise(new CreatedEvent(displayName, supplier, url));
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    _displayName = @event.DisplayName;

    Supplier = @event.Supplier;
    _url = @event.Url;
  }

  public void Delete()
  {
    if (!IsDeleted)
    {
      Raise(new DeletedEvent());
    }
  }

  public void SetPrice(decimal newPrice)
  {
    if (CurrentPrice != newPrice)
    {
      Raise(new PriceChanged(newPrice));
    }
  }
  protected virtual void Apply(PriceChanged @event)
  {
    CurrentPrice = @event.Price;
  }

  public void Unwatch()
  {
    if (IsBeingWatched)
    {
      Raise(new UnwatchedEvent());
    }
  }
  protected virtual void Apply(UnwatchedEvent _)
  {
    IsBeingWatched = false;
  }

  public void Watch()
  {
    if (!IsBeingWatched)
    {
      Raise(new WatchedEvent());
    }
  }
  protected virtual void Apply(WatchedEvent _)
  {
    IsBeingWatched = true;
  }

  public override string ToString() => $"{DisplayName} | {base.ToString()}";

  public class CreatedEvent : DomainEvent, INotification
  {
    public DisplayName DisplayName { get; }

    public Supplier Supplier { get; }
    public Uri Url { get; }

    public CreatedEvent(DisplayName displayName, Supplier supplier, Uri url)
    {
      DisplayName = displayName;

      Supplier = supplier;
      Url = url;
    }
  }

  public class DeletedEvent : DomainEvent, INotification
  {
    public DeletedEvent()
    {
      IsDeleted = true;
    }
  }

  public class PriceChanged : DomainEvent, INotification
  {
    public decimal Price { get; }

    public PriceChanged(decimal price)
    {
      Price = price;
    }
  }

  public class UnwatchedEvent : DomainEvent, INotification;

  public class WatchedEvent : DomainEvent, INotification;
}
