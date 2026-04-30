using EasyNetQ;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Infrastructure.MessageQueue;

public class RabbitMqEventPublisher(IBus bus) : IEventPublisher
{
    public async Task Publish(DomainEvent @event) => await bus.PubSub.PublishAsync(@event);
}