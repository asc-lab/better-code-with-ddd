using System;
using System.Collections.Generic;
using System.Linq;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.Asserts
{
    public class DomainEventsAssert
    {
        private readonly IEnumerable<DomainEvent> events;

        public DomainEventsAssert(IEnumerable<DomainEvent> events)
        {
            this.events = events;
        }

        public static DomainEventsAssert That(IEnumerable<DomainEvent> events)
        {
            return new DomainEventsAssert(events);
        }
        
        public DomainEventsAssert HasExpectedNumberOfEvents(int expectedNumberOfEvents)
        {
            Assert.Equal(expectedNumberOfEvents, events.Count());
            return this;
        }

        public DomainEventsAssert ContainsEvent<T>(Predicate<T> matcher) where T : DomainEvent
        {
            Assert.Contains(events, e => e.GetType() == typeof(T) && matcher((T)e));
            return this;
        }
    }
}