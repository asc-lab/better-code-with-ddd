using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly LoanDbContext dbContext;

    public EfUnitOfWork(LoanDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CommitChanges()
    {
        dbContext.SaveChanges();
    }
}