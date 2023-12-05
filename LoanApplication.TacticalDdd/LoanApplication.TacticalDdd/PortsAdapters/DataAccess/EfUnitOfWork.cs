using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class EfUnitOfWork(LoanDbContext dbContext) : IUnitOfWork
{
    public void CommitChanges()
    {
        dbContext.SaveChanges();
    }
}