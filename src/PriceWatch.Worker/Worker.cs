using MediatR;
using PriceWatch.Application.Products.Commands;
using PriceWatch.Infrastructure.Commands;

namespace PriceWatch.Worker;

internal class Worker : BackgroundService
{
  private readonly ILogger<Worker> _logger;
  private readonly IServiceProvider _serviceProvider;

  public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
  {
    _logger = logger;
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    // TODO(fpion): logging

    using IServiceScope scope = _serviceProvider.CreateScope();
    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    await mediator.Publish(new InitializeDatabaseCommand(), cancellationToken);
    await mediator.Send(new ImportProductsCommand(), cancellationToken);
  }
}
