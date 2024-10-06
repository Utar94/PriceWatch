using Microsoft.Extensions.DependencyInjection;

namespace PriceWatch.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPriceWatchApplication(this IServiceCollection services)
  {
    return services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
  }
}
