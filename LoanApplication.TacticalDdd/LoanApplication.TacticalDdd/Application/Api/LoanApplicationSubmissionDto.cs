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