using EasyNetQ;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.PortsAdapters.MessageQueue
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IBus bus;

        public RabbitMqEventPublisher(IBus bus)
        {
            this.bus = bus;
        }

        public void Publish(DomainEvent @event)
        {
            bus.Publish(@event);
        }
    }
}