using PriceWatch.EntityFrameworkCore.SqlServer;

namespace PriceWatch.Worker;

internal class Startup
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddHostedService<Worker>();
    services.AddPriceWatchWithEntityFrameworkCoreSqlServer(_configuration);
  }
}
