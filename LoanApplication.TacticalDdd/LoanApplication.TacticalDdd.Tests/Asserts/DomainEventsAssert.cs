using FluentAssertions;
using FluentAssertions.Execution;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Tests.Asserts;

public static class DomainEventsAssertExtension
{
    public static DomainEventsAssert Should(this IEnumerable<DomainEvent> events)
    {
        return new DomainEventsAssert(events);
    }
}
    
public class DomainEventsAssert
{
    private readonly IEnumerable<DomainEvent> Subject;
    public DomainEventsAssert(IEnumerable<DomainEvent> events)
    {
        Subject = events;
    }

    public AndConstraint<DomainEventsAssert> HaveExpectedNumberOfEvents(int expectedNumberOfEvents)
    {
        Subject.Count().Should().Be(expectedNumberOfEvents);
        return new AndConstraint<DomainEventsAssert>(this);
    }

    public AndConstraint<DomainEventsAssert> ContainEvent<T>(Predicate<T> matcher) where T : DomainEvent
    {
        using (new AssertionScope())
        {
            Subject.Any(e => e.GetType() == typeof(T) && matcher((T)e))
                .Should().BeTrue("List of events does not contain any that meets criteria");
        }
        return new AndConstraint<DomainEventsAssert>(this);
    }
}