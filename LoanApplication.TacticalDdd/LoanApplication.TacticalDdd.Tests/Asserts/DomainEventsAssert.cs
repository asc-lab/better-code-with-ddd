using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Tests.Asserts
{
    public static class DomainEventsAssertExtension
    {
        public static DomainEventsAssert Should(this IEnumerable<DomainEvent> events)
        {
            return new DomainEventsAssert(events);
        }
    }
    
    public class DomainEventsAssert : CollectionAssertions<IEnumerable<DomainEvent>,DomainEventsAssert>
    {
        public DomainEventsAssert(IEnumerable<DomainEvent> events) : base(events)
        {
        }

        public AndConstraint<DomainEventsAssert> HaveExpectedNumberOfEvents(int expectedNumberOfEvents)
        {
            Subject.Count().Should().Be(expectedNumberOfEvents);
            return new AndConstraint<DomainEventsAssert>(this);
        }

        public AndConstraint<DomainEventsAssert> ContainEvent<T>(Predicate<T> matcher) where T : DomainEvent
        {
            Execute.Assertion
                .ForCondition(Subject.Any(e => e.GetType() == typeof(T) && matcher((T) e)))
                .FailWith("List of events does not contain any that meets criteria");
            
            return new AndConstraint<DomainEventsAssert>(this);
        }

        protected override string Identifier => "DomainEventsAssert";
    }
}