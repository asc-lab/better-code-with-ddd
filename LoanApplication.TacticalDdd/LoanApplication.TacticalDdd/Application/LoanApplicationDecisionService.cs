using System.Security.Claims;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using LoanApplication.TacticalDdd.DomainModel.DomainEvents;

namespace LoanApplication.TacticalDdd.Application;

public class LoanApplicationDecisionService(IUnitOfWork unitOfWork, ILoanApplicationRepository loanApplications, IOperatorRepository operators, IEventPublisher eventPublisher)
{
    
    public async Task RejectApplication(string applicationNumber, ClaimsPrincipal principal, string rejectionReason)
    {
        var loanApplication = loanApplications.WithNumber(LoanApplicationNumber.Of(applicationNumber));
        var user = operators.WithLogin(Login.Of(principal.Identity.Name));
            
        loanApplication.Reject(user);
            
        unitOfWork.CommitChanges();
            
        await eventPublisher.Publish(new LoanApplicationRejected(loanApplication));
    }

    public async Task AcceptApplication(string applicationNumber, ClaimsPrincipal principal)
    {
        var loanApplication = loanApplications.WithNumber(LoanApplicationNumber.Of(applicationNumber));
        var user = operators.WithLogin(Login.Of(principal.Identity.Name));
            
        loanApplication.Accept(user);
            
        unitOfWork.CommitChanges();
            
        await eventPublisher.Publish(new LoanApplicationAccepted(loanApplication));
    }
}