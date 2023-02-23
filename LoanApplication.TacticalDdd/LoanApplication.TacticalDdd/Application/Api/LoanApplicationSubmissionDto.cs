using FluentValidation;

namespace LoanApplication.TacticalDdd.Application.Api;

public record LoanApplicationSubmissionDto
(
    string CustomerNationalIdentifier,
    string CustomerFirstName,
    string CustomerLastName,
    DateTime CustomerBirthdate,
    decimal CustomerMonthlyIncome,
    AddressDto CustomerAddress,
    decimal PropertyValue,
    AddressDto PropertyAddress,
    decimal LoanAmount,
    int LoanNumberOfYears,
    decimal InterestRate
);

public class LoanApplicationSubmissionDtoValidator : AbstractValidator<LoanApplicationSubmissionDto>
{
    public LoanApplicationSubmissionDtoValidator()
    {
        RuleFor(l => l.CustomerNationalIdentifier)
            .NotEmpty()
            .MinimumLength(11)
            .MaximumLength(11);

        RuleFor(l => l.CustomerFirstName)
            .NotEmpty();
        
        RuleFor(l => l.CustomerLastName)
            .NotEmpty();

        RuleFor(l => l.CustomerBirthdate)
            .NotEmpty();

        RuleFor(l => l.CustomerMonthlyIncome)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(l => l.PropertyValue)
            .GreaterThanOrEqualTo(0);
        
        
        RuleFor(l => l.LoanAmount)
            .GreaterThan(0);
        
        
        RuleFor(l => l.LoanNumberOfYears)
            .GreaterThan(0);
        
        RuleFor(l => l.InterestRate)
            .GreaterThan(0);

        RuleFor(l => l.CustomerAddress)
            .NotNull()
            .SetValidator(new AddressDtoValidator());
        
        RuleFor(l => l.PropertyAddress)
            .NotNull()
            .SetValidator(new AddressDtoValidator());
    }
}


public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.Country)
            .NotEmpty();
        
        RuleFor(a => a.ZipCode)
            .NotEmpty();
        
        RuleFor(a => a.City)
            .NotEmpty();
        
        RuleFor(a => a.Street)
            .NotEmpty();
    }
}