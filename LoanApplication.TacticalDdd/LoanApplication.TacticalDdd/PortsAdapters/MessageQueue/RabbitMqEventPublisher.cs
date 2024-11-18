using EasyNetQ;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.PortsAdapters.MessageQueue;

public class RabbitMqEventPublisher(IBus bus) : IEventPublisher
{
    public void Publish(DomainEvent @event)
    {
        bus.PubSub.Publish(@event);
    }
}