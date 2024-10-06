using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.Extensions.DependencyInjection;
using PriceWatch.Domain.Products;
using PriceWatch.EntityFrameworkCore.Repositories;
using PriceWatch.Infrastructure;

namespace PriceWatch.EntityFrameworkCore;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPriceWatchWithEntityFrameworkCore(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddPriceWatchInfrastructure()
      .AddRepositories();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services.AddScoped<IProductRepository, ProductRepository>();
  }
}
