using LoanApplication.TacticalDdd.DomainModel;
using Marten;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class MartenLoanApplicationRepository : ILoanApplicationRepository
{
    private readonly IDocumentSession documentSession;

    public MartenLoanApplicationRepository(IDocumentSession documentSession)
    {
        this.documentSession = documentSession;
    }

    public void Add(DomainModel.LoanApplication loanApplication)
    {
        documentSession.Insert(loanApplication);
    }

    public DomainModel.LoanApplication WithNumber(string loanApplicationNumber)
    {
        return documentSession.Query<DomainModel.LoanApplication>()
            .FirstOrDefault(l => l.Number == loanApplicationNumber);
    }
}