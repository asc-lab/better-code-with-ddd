namespace LoanApplication.TacticalDdd.DomainModel.Ddd;

public interface IEventPublisher
{
    Task Publish(DomainEvent @event);
}