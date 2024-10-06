using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using PriceWatch.Domain.Products;

namespace PriceWatch.EntityFrameworkCore.Repositories;

internal class ProductRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IProductRepository
{
  public ProductRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer)
    : base(eventBus, eventContext, eventSerializer)
  {
  }

  public async Task<IReadOnlyCollection<Product>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<Product>(cancellationToken)).ToArray().AsReadOnly();
  }

  public async Task SaveAsync(Product product, CancellationToken cancellationToken)
  {
    await base.SaveAsync(product, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Product> products, CancellationToken cancellationToken)
  {
    await base.SaveAsync(products, cancellationToken);
  }
}
