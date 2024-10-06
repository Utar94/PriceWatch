using MediatR;
using PriceWatch.Application.Products.Handlers;
using PriceWatch.Domain.Products;

namespace PriceWatch.Application.Products.Commands;

public record WatchProductsCommand : IRequest;

internal class WatchProductsCommandHandler : IRequestHandler<WatchProductsCommand>
{
  private readonly IPriceHandlerFactory _priceHandlerFactory;
  private readonly IProductRepository _productRepository;

  public WatchProductsCommandHandler(IPriceHandlerFactory priceHandlerFactory, IProductRepository productRepository)
  {
    _priceHandlerFactory = priceHandlerFactory;
    _productRepository = productRepository;
  }

  public async Task Handle(WatchProductsCommand _, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Product> products = await _productRepository.LoadAsync(cancellationToken);

    foreach (Product product in products)
    {
      if (product.IsBeingWatched)
      {
        IPriceHandler handler = _priceHandlerFactory.GetHandler(product);
        await handler.UpdateAsync(product, cancellationToken);
      }
    }

    await _productRepository.SaveAsync(products, cancellationToken);
  }
}
