using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PriceWatch.Infrastructure.Commands;

namespace PriceWatch.EntityFrameworkCore.Handlers;

internal class InitializeDatabaseCommandHandler : INotificationHandler<InitializeDatabaseCommand>
{
  private readonly bool _enableMigrations;
  private readonly EventContext _eventContext;
  private readonly PriceWatchContext _priceWatchContext;

  public InitializeDatabaseCommandHandler(IConfiguration configuration, EventContext eventContext, PriceWatchContext priceWatchContext)
  {
    _enableMigrations = configuration.GetValue<bool>("EnableMigrations");
    _eventContext = eventContext;
    _priceWatchContext = priceWatchContext;
  }

  public async Task Handle(InitializeDatabaseCommand _, CancellationToken cancellationToken)
  {
    await _eventContext.Database.MigrateAsync(cancellationToken);
    await _priceWatchContext.Database.MigrateAsync(cancellationToken);
  }
}
