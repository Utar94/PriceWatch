using PriceWatch.Domain.Products;

namespace PriceWatch.Application.Products.Handlers;

internal interface IPriceHandlerFactory
{
  IPriceHandler GetHandler(Product product);
}
