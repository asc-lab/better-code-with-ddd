using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Tests.Mocks;

public class UnitOfWorkMock : IUnitOfWork
{
    public void CommitChanges()
    {
        //do nothing
    }
}