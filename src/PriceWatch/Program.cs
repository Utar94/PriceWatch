using HtmlAgilityPack;

namespace PriceWatch;

internal class Program
{
  private const string Url = "https://www.long-mcquade.com/16419/Drums/Cymbals/Sabian/AA-17-Inch-Holy-China-Traditional-Finish.htm";

  static void Main()
  {
    Uri uri = new(Url, UriKind.Absolute);
    Console.WriteLine(uri.Host);

    HtmlWeb web = new();
    HtmlDocument document = web.Load(Url);

    HtmlNode titleNode = document.DocumentNode.SelectSingleNode("//head/title");
    Console.WriteLine(titleNode.InnerText);

    HtmlNode node = document.GetElementbyId("product-regular-price");
    Console.WriteLine(node.InnerText);
  }
}
