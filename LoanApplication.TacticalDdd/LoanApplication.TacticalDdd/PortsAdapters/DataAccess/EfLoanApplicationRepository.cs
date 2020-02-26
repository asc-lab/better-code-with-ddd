using System.Linq;
using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess
{
    public class EfLoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly LoanDbContext dbContext;

        public EfLoanApplicationRepository(LoanDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(DomainModel.LoanApplication loanApplication)
        {
            dbContext.LoanApplications.Add(loanApplication);
        }

        public DomainModel.LoanApplication WithNumber(LoanApplicationNumber loanApplicationNumber)
        {
            return dbContext.LoanApplications.FirstOrDefault(l => l.Number == loanApplicationNumber);
        }
    }
}