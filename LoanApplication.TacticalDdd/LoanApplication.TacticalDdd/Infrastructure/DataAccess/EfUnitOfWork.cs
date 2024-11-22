using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Infrastructure.DataAccess;

public class EfUnitOfWork(LoanDbContext dbContext) : IUnitOfWork
{
    public void CommitChanges()
    {
        dbContext.SaveChanges();
    }
}