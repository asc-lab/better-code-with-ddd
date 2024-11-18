namespace LoanApplication.TacticalDdd.DomainModel;

public class ScoringRulesFactory(IDebtorRegistry registry)
{
    public ScoringRules DefaultSet => new(new List<IScoringRule>
    {
        new LoanAmountMustBeLowerThanPropertyValue(),
        new CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65(),
        new InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome(),
        new CustomerIsNotARegisteredDebtor(registry)
    });
}