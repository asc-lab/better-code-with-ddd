using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class EfOperatorRepository(LoanDbContext dbContext) : IOperatorRepository
{
    public void Add(Operator @operator)
    {
        dbContext.Operators.Add(@operator);
    }

    public Operator WithLogin(Login login)
    {
        return dbContext.Operators.FirstOrDefault(o => o.Login == login);
    }
}