using MediatR;
using PriceWatch.Domain.Products;

namespace PriceWatch.Application.Products.Commands;

public record ImportProductsCommand : IRequest;

internal class ImportProductsCommandHandler : IRequestHandler<ImportProductsCommand>
{
  private const string Path = "products.json";

  private readonly JsonSerializerOptions _options = new();

  private readonly IProductRepository _productRepository;

  public ImportProductsCommandHandler(IProductRepository productRepository)
  {
    _options.Converters.Add(new JsonStringEnumConverter());

    _productRepository = productRepository;
  }

  public async Task Handle(ImportProductsCommand _, CancellationToken cancellationToken)
  {
    Dictionary<Guid, Product> products = (await _productRepository.LoadAsync(cancellationToken))
      .ToDictionary(x => x.Id.AggregateId.ToGuid(), x => x);

    IReadOnlyCollection<ProductData> extractedProducts = await ExtractAsync(cancellationToken);

    HashSet<Guid> ids = extractedProducts.Select(p => p.Id).ToHashSet();
    foreach (KeyValuePair<Guid, Product> product in products)
    {
      if (!ids.Contains(product.Key))
      {
        product.Value.Delete();
      }
    }

    foreach (ProductData extractedProduct in extractedProducts)
    {
      if (!products.TryGetValue(extractedProduct.Id, out Product? product))
      {
        DisplayName displayName = new(extractedProduct.DisplayName);
        Uri url = new(extractedProduct.Url.Trim());
        product = new(displayName, extractedProduct.Supplier, url, extractedProduct.Id);
        products[extractedProduct.Id] = product;
      }

      if (extractedProduct.IsBeingWatched)
      {
        product.Watch();
      }
      else
      {
        product.Unwatch();
      }
    }

    await _productRepository.SaveAsync(products.Values, cancellationToken);
  }

  private async Task<IReadOnlyCollection<ProductData>> ExtractAsync(CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync(Path, cancellationToken);
    return (JsonSerializer.Deserialize<ProductData[]>(json, _options) ?? []).AsReadOnly();
  }
}

internal record ProductData
{
  [JsonPropertyName("id")]
  public Guid Id { get; set; }

  [JsonPropertyName("name")]
  public string DisplayName { get; set; } = string.Empty;

  [JsonPropertyName("supplier")]
  public Supplier Supplier { get; set; }

  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;

  [JsonPropertyName("watch")]
  public bool IsBeingWatched { get; set; }
}
