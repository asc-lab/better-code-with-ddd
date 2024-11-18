using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using MediatR;

namespace LoanApplication.TacticalDdd.Application;

public static class EvaluateLoanApplication
{
    public class Command : IRequest<Unit>
    {
        public string ApplicationNumber { get; set; }
    }


    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IDebtorRegistry debtorRegistry;
        private readonly ILoanApplicationRepository loanApplications;
        private readonly ScoringRulesFactory scoringRulesFactory;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IUnitOfWork unitOfWork, ILoanApplicationRepository loanApplications,
            IDebtorRegistry debtorRegistry)
        {
            this.unitOfWork = unitOfWork;
            this.loanApplications = loanApplications;
            this.debtorRegistry = debtorRegistry;
            scoringRulesFactory = new ScoringRulesFactory(debtorRegistry);
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loanApplication = loanApplications.WithNumber(request.ApplicationNumber);

            loanApplication.Evaluate(scoringRulesFactory.DefaultSet);

            unitOfWork.CommitChanges();

            return Task.FromResult(Unit.Value);
        }
    }
}