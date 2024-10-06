using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceWatch.Domain.Products;
using PriceWatch.EntityFrameworkCore.Entities;

namespace PriceWatch.EntityFrameworkCore.Handlers;

internal static class ProductEvents
{
  public class ProductCreatedEventHandler : INotificationHandler<Product.CreatedEvent>
  {
    private readonly PriceWatchContext _context;

    public ProductCreatedEventHandler(PriceWatchContext context)
    {
      _context = context;
    }

    public async Task Handle(Product.CreatedEvent @event, CancellationToken cancellationToken)
    {
      ProductEntity? product = await _context.Products.AsNoTracking()
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (product == null)
      {
        product = new(@event);

        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class ProductDeletedEventHandler : INotificationHandler<Product.DeletedEvent>
  {
    private readonly PriceWatchContext _context;

    public ProductDeletedEventHandler(PriceWatchContext context)
    {
      _context = context;
    }

    public async Task Handle(Product.DeletedEvent @event, CancellationToken cancellationToken)
    {
      ProductEntity? product = await _context.Products
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
      if (product != null)
      {
        _context.Products.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }

  public class ProductPriceChangedHandler : INotificationHandler<Product.PriceChanged>
  {
    private readonly PriceWatchContext _context;

    public ProductPriceChangedHandler(PriceWatchContext context)
    {
      _context = context;
    }

    public async Task Handle(Product.PriceChanged @event, CancellationToken cancellationToken)
    {
      ProductEntity product = await _context.Products
        .Include(x => x.PriceHistory)
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The product entity 'AggregateId={@event.AggregateId}' could not be found.");

      product.SetPrice(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public class ProductUnwatchedEventHandler : INotificationHandler<Product.UnwatchedEvent>
  {
    private readonly PriceWatchContext _context;

    public ProductUnwatchedEventHandler(PriceWatchContext context)
    {
      _context = context;
    }

    public async Task Handle(Product.UnwatchedEvent @event, CancellationToken cancellationToken)
    {
      ProductEntity product = await _context.Products
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The product entity 'AggregateId={@event.AggregateId}' could not be found.");

      product.Unwatch(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public class ProductWatchedEventHandler : INotificationHandler<Product.WatchedEvent>
  {
    private readonly PriceWatchContext _context;

    public ProductWatchedEventHandler(PriceWatchContext context)
    {
      _context = context;
    }

    public async Task Handle(Product.WatchedEvent @event, CancellationToken cancellationToken)
    {
      ProductEntity product = await _context.Products
        .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The product entity 'AggregateId={@event.AggregateId}' could not be found.");

      product.Watch(@event);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
