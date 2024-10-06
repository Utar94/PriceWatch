using Logitar.EventSourcing;

namespace PriceWatch.Domain.Products;

public readonly struct ProductId
{
  public AggregateId AggregateId { get; }

  public ProductId() : this(AggregateId.NewId())
  {
  }
  public ProductId(Guid id) : this(new AggregateId(id))
  {
  }
  public ProductId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }
}
