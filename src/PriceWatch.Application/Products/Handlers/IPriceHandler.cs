using PriceWatch.Domain.Products;

namespace PriceWatch.Application.Products.Handlers;

internal interface IPriceHandler
{
  Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
}
