using System.Security.Claims;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Application;

public class LoanApplicationSubmissionService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ILoanApplicationRepository loanApplications;
    private readonly IOperatorRepository operators;

    public LoanApplicationSubmissionService(IUnitOfWork unitOfWork,ILoanApplicationRepository loanApplications, IOperatorRepository operators)
    {
        this.unitOfWork = unitOfWork;
        this.loanApplications = loanApplications;
        this.operators = operators;
    }
        
    public string SubmitLoanApplication(LoanApplicationSubmissionDto loanApplicationDto, ClaimsPrincipal principal)
    {
        var user = operators.WithLogin(Login.Of(principal.Identity.Name));
            
        var application = new DomainModel.LoanApplication
        (
            LoanApplicationNumber.NewNumber(),
            new Customer
            (
                new NationalIdentifier(loanApplicationDto.CustomerNationalIdentifier),
                new Name(loanApplicationDto.CustomerFirstName, loanApplicationDto.CustomerLastName),
                DateOnly.FromDateTime(loanApplicationDto.CustomerBirthdate),
                new MonetaryAmount(loanApplicationDto.CustomerMonthlyIncome),
                new Address
                (
                    loanApplicationDto.CustomerAddress.Country,
                    loanApplicationDto.CustomerAddress.ZipCode,
                    loanApplicationDto.CustomerAddress.City,
                    loanApplicationDto.CustomerAddress.Street
                )
            ), 
            new Property
            (
                new MonetaryAmount(loanApplicationDto.PropertyValue),
                new Address
                (
                    loanApplicationDto.PropertyAddress.Country,
                    loanApplicationDto.PropertyAddress.ZipCode,
                    loanApplicationDto.PropertyAddress.City,
                    loanApplicationDto.PropertyAddress.Street
                )
            ), 
            new Loan
            (
                new MonetaryAmount(loanApplicationDto.LoanAmount),
                loanApplicationDto.LoanNumberOfYears,
                new Percent(loanApplicationDto.InterestRate)
            ),
            user
        );
            
        loanApplications.Add(application);
            
        unitOfWork.CommitChanges();

        return application.Number;
            
    }
        
}