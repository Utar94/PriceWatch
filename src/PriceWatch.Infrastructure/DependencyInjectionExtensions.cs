using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PriceWatch.Application;
using PriceWatch.Infrastructure.Converters;

namespace PriceWatch.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPriceWatchInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddPriceWatchApplication()
      .AddSingleton<IEventSerializer>(InitializeEventSerializer)
      .AddTransient<IEventBus, EventBus>();
  }

  private static EventSerializer InitializeEventSerializer(IServiceProvider serviceProvider) => new(serviceProvider.GetJsonConverters());
  public static IEnumerable<JsonConverter> GetJsonConverters(this IServiceProvider _) =>
  [
    new DisplayNameConverter()
  ];
}
