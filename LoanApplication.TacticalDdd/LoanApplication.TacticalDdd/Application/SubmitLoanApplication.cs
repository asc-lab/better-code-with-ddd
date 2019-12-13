using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using MediatR;

namespace LoanApplication.TacticalDdd.Application
{
    public class SubmitLoanApplication
    {
        public class Command : IRequest<string>
        {
            public LoanApplicationDto LoanApplication { get; set; }
            public ClaimsPrincipal CurrentUser { get; set; }
        }
        
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ILoanApplicationRepository loanApplications;
            private readonly IOperatorRepository operators;

            public Handler(IUnitOfWork unitOfWork, ILoanApplicationRepository loanApplications, IOperatorRepository operators)
            {
                this.unitOfWork = unitOfWork;
                this.loanApplications = loanApplications;
                this.operators = operators;
            }

            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = operators.WithLogin(request.CurrentUser.Identity.Name);
            
                var application = new DomainModel.LoanApplication
                (
                    Guid.NewGuid().ToString(),
                    new Customer
                    (
                        new NationalIdentifier(request.LoanApplication.CustomerNationalIdentifier),
                        new Name(request.LoanApplication.CustomerFirstName, request.LoanApplication.CustomerLastName),
                        request.LoanApplication.CustomerBirthdate,
                        new MonetaryAmount(request.LoanApplication.CustomerMonthlyIncome),
                        new Address
                        (
                            request.LoanApplication.CustomerAddress.Country,
                            request.LoanApplication.CustomerAddress.ZipCode,
                            request.LoanApplication.CustomerAddress.City,
                            request.LoanApplication.CustomerAddress.Street
                        )
                    ), 
                    new Property
                    (
                        new MonetaryAmount(request.LoanApplication.PropertyValue),
                        new Address
                        (
                            request.LoanApplication.PropertyAddress.Country,
                            request.LoanApplication.PropertyAddress.ZipCode,
                            request.LoanApplication.PropertyAddress.City,
                            request.LoanApplication.PropertyAddress.Street
                        )
                    ), 
                    new Loan
                    (
                        new MonetaryAmount(request.LoanApplication.LoanAmount),
                        request.LoanApplication.LoanNumberOfYears,
                        new Percent(request.LoanApplication.InterestRate)
                    ),
                    user
                );
            
                loanApplications.Add(application);
            
                unitOfWork.CommitChanges();

                return Task.FromResult(application.Number);
            }
        }
    }
}