using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PriceWatch.EntityFrameworkCore.SqlServer;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "SQLCONNSTR_PriceWatch";

  public static IServiceCollection AddPriceWatchWithEntityFrameworkCoreSqlServer(this IServiceCollection services, IConfiguration configuration)
  {
    string? connectionString = Environment.GetEnvironmentVariable(ConfigurationKey);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      connectionString = configuration.GetValue<string>(ConfigurationKey);
    }
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new ArgumentException($"The configuration '{ConfigurationKey}' could not be found.", nameof(configuration));
    }
    return services.AddPriceWatchWithEntityFrameworkCoreSqlServer(connectionString.Trim());
  }
  public static IServiceCollection AddPriceWatchWithEntityFrameworkCoreSqlServer(this IServiceCollection services, string connectionString)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddPriceWatchWithEntityFrameworkCore()
      .AddDbContext<PriceWatchContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("PriceWatch.EntityFrameworkCore.SqlServer")));
  }
}
