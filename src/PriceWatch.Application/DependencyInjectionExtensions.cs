using Microsoft.Extensions.DependencyInjection;
using PriceWatch.Application.Products.Handlers;

namespace PriceWatch.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPriceWatchApplication(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddSingleton<IPriceHandlerFactory, PriceHandlerFactory>();
  }
}
