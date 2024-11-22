using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Infrastructure.DataAccess;

public class EfLoanApplicationRepository(LoanDbContext dbContext) : ILoanApplicationRepository
{
    public void Add(DomainModel.LoanApplication loanApplication) => dbContext.LoanApplications.Add(loanApplication);
    
    public DomainModel.LoanApplication WithNumber(LoanApplicationNumber loanApplicationNumber) =>
        dbContext.LoanApplications.FirstOrDefault(l => l.Number == loanApplicationNumber);
}