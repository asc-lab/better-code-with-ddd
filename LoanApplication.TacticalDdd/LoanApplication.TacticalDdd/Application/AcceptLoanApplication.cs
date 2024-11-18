using System.Security.Claims;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using LoanApplication.TacticalDdd.DomainModel.DomainEvents;
using MediatR;

namespace LoanApplication.TacticalDdd.Application;

public static class AcceptLoanApplication
{
    public class Command : IRequest<Unit>
    {
        public string ApplicationNumber { get; set; }

        public ClaimsPrincipal CurrentUser { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IEventPublisher eventPublisher;
        private readonly ILoanApplicationRepository loanApplications;
        private readonly IOperatorRepository operators;
        private readonly IUnitOfWork unitOfWork;

        public Handler(
            IUnitOfWork unitOfWork,
            ILoanApplicationRepository loanApplications,
            IOperatorRepository operators,
            IEventPublisher eventPublisher)
        {
            this.unitOfWork = unitOfWork;
            this.loanApplications = loanApplications;
            this.operators = operators;
            this.eventPublisher = eventPublisher;
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loanApplication = loanApplications.WithNumber(request.ApplicationNumber);
            var user = operators.WithLogin(request.CurrentUser.Identity.Name);

            loanApplication.Accept(user);

            unitOfWork.CommitChanges();

            eventPublisher.Publish(new LoanApplicationAccepted(loanApplication));

            return Task.FromResult(Unit.Value);
        }
    }
}