using MediatR;

namespace ECommerce.Common.Abstractions.Messaging;

public interface IMessageBusConsumer<TCommand>
    where TCommand : IRequest
{
    Task ConsumeAsync(string queue, CancellationToken cancellationToken);
}
