namespace LoanApplication.TacticalDdd.DomainModel.Ddd
{
    public interface IEventPublisher
    {
        void Publish(DomainEvent @event);
    }
}