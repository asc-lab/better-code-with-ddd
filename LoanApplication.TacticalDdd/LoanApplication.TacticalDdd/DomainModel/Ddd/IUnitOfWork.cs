namespace LoanApplication.TacticalDdd.DomainModel.Ddd;

public interface IUnitOfWork
{
    void CommitChanges();
}