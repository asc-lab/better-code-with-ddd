using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Infrastructure.DataAccess;

public class EfOperatorRepository(LoanDbContext dbContext) : IOperatorRepository
{
    public void Add(Operator @operator) => dbContext.Operators.Add(@operator);

    public Operator WithLogin(Login login) => dbContext.Operators.FirstOrDefault(o => o.Login == login);

}