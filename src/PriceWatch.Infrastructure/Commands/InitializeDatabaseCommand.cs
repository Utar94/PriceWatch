using MediatR;

namespace PriceWatch.Infrastructure.Commands;

public record InitializeDatabaseCommand : INotification;
