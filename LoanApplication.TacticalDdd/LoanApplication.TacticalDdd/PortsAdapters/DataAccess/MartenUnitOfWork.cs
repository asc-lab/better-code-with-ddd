using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Marten;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class MartenUnitOfWork : IUnitOfWork
{
    private readonly IDocumentSession documentSession;

    public MartenUnitOfWork(IDocumentSession documentSession)
    {
        this.documentSession = documentSession;
    }


    public void CommitChanges()
    {
        documentSession.SaveChanges();
    }
}