namespace PriceWatch.Domain.Products;

public interface IProductRepository
{
  Task<IReadOnlyCollection<Product>> LoadAsync(CancellationToken cancellationToken = default);

  Task SaveAsync(Product product, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default);
}
