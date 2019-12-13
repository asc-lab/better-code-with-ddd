using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Tests.Mocks
{
    public class InMemoryBus : IEventPublisher
    {
        private readonly List<DomainEvent> events = new List<DomainEvent>();
        public void Publish(DomainEvent @event)
        {
            events.Add(@event);
        }

        public ReadOnlyCollection<DomainEvent> Events => events.AsReadOnly();
    }
}