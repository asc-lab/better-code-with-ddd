using FluentValidation;

namespace LoanApplication.BusinessLogic
{
    public class ValidationService
    {
        public void ValidateLoanApplication(LoanApplication loanApplication)
        {
            new LoanApplicationValidator().ValidateAndThrow(loanApplication);
        }

        class LoanApplicationValidator : AbstractValidator<LoanApplication>
        {
            public LoanApplicationValidator()
            {
                RuleFor(x => x.Number).NotEmpty();
                RuleFor(x => x.Customer).NotEmpty().SetValidator(new CustomerValidator());
                RuleFor(x => x.Property).NotEmpty().SetValidator(new PropertyValidator());
                RuleFor(x => x.LoanAmount).NotEmpty().GreaterThan(0M);
                RuleFor(x => x.LoanNumberOfYears).NotEmpty().GreaterThanOrEqualTo(1);
                RuleFor(x => x.InterestRate).NotEmpty().GreaterThan(0M);
                RuleFor(x => x.RegisteredBy).NotEmpty();
                RuleFor(x => x.RegistrationDate).NotEmpty();
            }
        }

        class CustomerValidator : AbstractValidator<Customer>
        {
            public CustomerValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.NationalIdentifier).NotEmpty().MinimumLength(11).MaximumLength(11);
                RuleFor(x => x.Birthdate).NotEmpty();
                RuleFor(x => x.MonthlyIncome).NotEmpty();
                RuleFor(x => x.Address).NotEmpty().SetValidator(new AddressValidator());
            }
        }

        class PropertyValidator : AbstractValidator<Property>
        {
            public PropertyValidator()
            {
                RuleFor(x => x.Value).NotEmpty().GreaterThan(0M);
                RuleFor(x => x.Address).NotEmpty().SetValidator(new AddressValidator());
            }
        }

        class AddressValidator : AbstractValidator<Address>
        {
            public AddressValidator()
            {
                RuleFor(x => x.Country).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Street).NotEmpty();
                RuleFor(x => x.ZipCode).NotEmpty().Matches("[0-9]{2}-[0-9]{3}");

            }
        }
    }
}