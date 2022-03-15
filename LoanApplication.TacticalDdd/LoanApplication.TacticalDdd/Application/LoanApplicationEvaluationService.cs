namespace LoanApplication.TacticalDdd.Application;

using DomainModel;
using DomainModel.Ddd;

public class LoanApplicationEvaluationService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ILoanApplicationRepository loanApplications;
    private readonly ScoringRulesFactory scoringRulesFactory;
        
    public LoanApplicationEvaluationService(
        IUnitOfWork unitOfWork,
        ILoanApplicationRepository loanApplications, 
        IDebtorRegistry debtorRegistry)
    {
        this.unitOfWork = unitOfWork;
        this.loanApplications = loanApplications;
        this.scoringRulesFactory = new ScoringRulesFactory(debtorRegistry);
    }
    public void EvaluateLoanApplication(string applicationNumber)
    {
        var loanApplication = loanApplications.WithNumber(LoanApplicationNumber.Of(applicationNumber));
            
        loanApplication.Evaluate(scoringRulesFactory.DefaultSet);
            
        unitOfWork.CommitChanges();
    }
}