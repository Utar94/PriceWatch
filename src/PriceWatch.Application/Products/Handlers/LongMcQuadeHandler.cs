using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using PriceWatch.Domain.Products;

namespace PriceWatch.Application.Products.Handlers;

internal class LongMcQuadeHandler : IPriceHandler
{
  private readonly ILogger<LongMcQuadeHandler> _logger;

  public LongMcQuadeHandler(ILogger<LongMcQuadeHandler> logger)
  {
    _logger = logger;
  }

  public Task UpdateAsync(Product product, CancellationToken _)
  {
    Update(product);
    return Task.CompletedTask;
  }
  private void Update(Product product)
  {
    HtmlWeb web = new();
    HtmlDocument? document = web.Load(product.Url);
    if (document == null)
    {
      _logger.LogWarning("The HTML document could not be loaded for '{Product}'.", product);
      return;
    }

    decimal? price = GetPriceFromNode(document, "product-regular-price", product)
      ?? GetPriceFromNode(document, "product-sale-price", product)
      ?? GetPriceFromNode(document, "product-original-price", product);

    if (price.HasValue)
    {
      product.SetPrice(price.Value);
      _logger.LogDebug("The price '{Price}' has been set for '{Product}'.", price, product);
    }
    else
    {
      _logger.LogWarning("No price could be found for '{Product}'.", product);
    }
  }
  private decimal? GetPriceFromNode(HtmlDocument document, string id, Product product)
  {
    decimal price;

    HtmlNode? node = document.GetElementbyId(id);
    if (node == null)
    {
      _logger.LogDebug("The HTML node 'Id={Id}' could not be found for '{Product}'.", id, product);
      return null;
    }
    else if (!decimal.TryParse(node.InnerText.Trim('$'), NumberStyles.Currency, CultureInfo.InvariantCulture, out price))
    {
      _logger.LogDebug("The price could not be parsed from HTML node 'Id={Id}, InnerText={InnerText}' for '{Product}'.", id, node.InnerText, product);
      return null;
    }

    _logger.LogDebug("The price '{Price}' was parsed from HTML node 'Id={Id}' from '{Product}'.", price, id, product);
    return price;
  }
}
