using System.Collections.Concurrent;
using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Mocks;

public class InMemoryOperatorRepository : IOperatorRepository
{
    private readonly ConcurrentDictionary<Guid, Operator> operators = new();

    public InMemoryOperatorRepository(IEnumerable<Operator> initialData)
    {
        foreach (var @operator in initialData) operators[@operator.Id] = @operator;
    }

    public void Add(Operator @operator)
    {
        operators[@operator.Id] = @operator;
    }

    public Operator WithLogin(string login)
    {
        return operators.Values.FirstOrDefault(o => o.Login == login);
    }
}