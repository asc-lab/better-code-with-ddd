using System.Collections.Generic;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class ScoringRulesFactory
    {
        private readonly IDebtorRegistry debtorRegistry;

        public ScoringRulesFactory(IDebtorRegistry debtorRegistry)
        {
            this.debtorRegistry = debtorRegistry;
        }
        
        public ScoringRules DefaultSet => new ScoringRules(new List<IScoringRule>
        {
            new LoanAmountMustBeLowerThanPropertyValue(),
            new CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65(),
            new InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome(),
            new CustomerIsNotARegisteredDebtor(debtorRegistry)
        });
    }
}