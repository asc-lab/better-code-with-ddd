namespace LoanApplication.TacticalDdd.Application;

using DomainModel;
using DomainModel.Ddd;

public class LoanApplicationEvaluationService(
    IUnitOfWork unitOfWork,
    ILoanApplicationRepository loanApplications,
    IDebtorRegistry debtorRegistry)
{
    private readonly ScoringRulesFactory scoringRulesFactory = new(debtorRegistry);

    public void EvaluateLoanApplication(string applicationNumber)
    {
        var loanApplication = loanApplications.WithNumber(LoanApplicationNumber.Of(applicationNumber));
            
        loanApplication.Evaluate(scoringRulesFactory.DefaultSet);
            
        unitOfWork.CommitChanges();
    }
}