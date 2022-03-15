using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class EfOperatorRepository : IOperatorRepository
{
    private readonly LoanDbContext dbContext;

    public EfOperatorRepository(LoanDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Operator @operator)
    {
        dbContext.Operators.Add(@operator);
    }

    public Operator WithLogin(Login login)
    {
        return dbContext.Operators.FirstOrDefault(o => o.Login == login);
    }
}