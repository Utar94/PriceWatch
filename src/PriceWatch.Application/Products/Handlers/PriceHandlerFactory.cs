using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PriceWatch.Domain.Products;

namespace PriceWatch.Application.Products.Handlers;

internal class PriceHandlerFactory : IPriceHandlerFactory
{
  private readonly Dictionary<Supplier, IPriceHandler> _handlers = [];

  public PriceHandlerFactory(IServiceProvider serviceProvider)
  {
    _handlers[Supplier.LongMcQuade] = new LongMcQuadeHandler(serviceProvider.GetRequiredService<ILogger<LongMcQuadeHandler>>());
  }

  public IPriceHandler GetHandler(Product product)
  {
    return _handlers.TryGetValue(product.Supplier, out IPriceHandler? handler)
      ? handler
      : throw new ArgumentException($"The supplier '{product.Supplier}' is not supported.", nameof(product));
  }
}
