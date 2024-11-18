using LoanApplication.TacticalDdd.DomainModel;
using Marten;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class MartenOperatorRepository : IOperatorRepository
{
    private readonly IDocumentSession documentSession;

    public MartenOperatorRepository(IDocumentSession documentSession)
    {
        this.documentSession = documentSession;
    }

    public void Add(Operator @operator)
    {
        documentSession.Insert(@operator);
    }

    public Operator WithLogin(string login)
    {
        return documentSession.Query<Operator>()
            .FirstOrDefault(o => o.Login == login);
    }
}