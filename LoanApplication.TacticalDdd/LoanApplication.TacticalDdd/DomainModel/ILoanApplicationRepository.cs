namespace LoanApplication.TacticalDdd.DomainModel
{
    public interface ILoanApplicationRepository
    {
        void Add(LoanApplication loanApplication);

        LoanApplication WithNumber(LoanApplicationNumber loanApplicationNumber);
    }
}