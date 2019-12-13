namespace LoanApplication.TacticalDdd.DomainModel
{
    public interface IDebtorRegistry
    {
        bool IsRegisteredDebtor(Customer customer);
    }
}