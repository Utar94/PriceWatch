using MediatR;
using PriceWatch.Application.Products.Commands;
using PriceWatch.Infrastructure.Commands;
using System.Diagnostics;

namespace PriceWatch.Worker;

internal class Worker : BackgroundService
{
  private const string GenericErrorMessage = "An unhanded exception occurred.";

  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger<Worker> _logger;
  private readonly IServiceProvider _serviceProvider;

  private LogLevel _result = LogLevel.Information; // NOTE(fpion): "Information" means success.

  public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger, IServiceProvider serviceProvider)
  {
    _hostApplicationLifetime = hostApplicationLifetime;
    _logger = logger;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    Stopwatch chrono = Stopwatch.StartNew();
    _logger.LogInformation("Worker executing at {Timestamp}.", DateTimeOffset.Now);

    using IServiceScope scope = _serviceProvider.CreateScope();
    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    try
    {
      await mediator.Publish(new InitializeDatabaseCommand(), cancellationToken);
      await mediator.Send(new ImportProductsCommand(), cancellationToken);
      await mediator.Send(new WatchProductsCommand(), cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, GenericErrorMessage);
      _result = LogLevel.Error;

      Environment.ExitCode = exception.HResult;
    }

    chrono.Stop();

    long seconds = chrono.ElapsedMilliseconds / 1000;
    string secondText = seconds <= 1 ? "second" : "seconds";
    switch (_result)
    {
      case LogLevel.Error:
        _logger.LogError("ETL failed after {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
        break;
      case LogLevel.Warning:
        _logger.LogWarning("ETL completed with warnings in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
        break;
      default:
        _logger.LogInformation("ETL succeeded in {Elapsed}ms ({Seconds} {SecondText}).", chrono.ElapsedMilliseconds, seconds, secondText);
        break;
    }

    _hostApplicationLifetime.StopApplication();
  }
}
