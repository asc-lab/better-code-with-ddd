using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanApplication.BusinessLogic
{
    public class ScoringService
    {
        private readonly IList<string> messages = new List<string>();
        private readonly DebtorRegistryClient debtorRegistryClient;

        public ScoringService(DebtorRegistryClient debtorRegistryClient)
        {
            this.debtorRegistryClient = debtorRegistryClient;
        }

        public async Task EvaluateApplication(LoanApplication loanApplication)
        {
            var score = ApplicationScore.Green;
            var explanation = new List<string>();
            
            //property value
            if (loanApplication.Property.Value < loanApplication.LoanAmount)
            {
                score = ApplicationScore.Red;
                explanation.Add("Property value is lower than loan amount.");
            }
            
            //max age
            if (DateTime.Now.Year + loanApplication.LoanNumberOfYears - loanApplication.Customer.Birthdate.Year > 65)
            {
                score = ApplicationScore.Red;
                explanation.Add("Customer age at last installment date is above 65");
            }
            
            //income vs installment
            if (loanApplication.LoanAmount / (loanApplication.LoanNumberOfYears * 12) >
                loanApplication.Customer.MonthlyIncome * 0.15M)
            {
                score = ApplicationScore.Red;
                explanation.Add("Installment is higher than 15% of customer's income");
            }

            //is debtor
            var debtorInfo = await debtorRegistryClient.GetDebtorInfo(loanApplication.Customer.NationalIdentifier);
            if (debtorInfo.Debts.Any())
            {
                score = ApplicationScore.Red;
                explanation.Add("Customer is registered in debtor registry");
            }

            loanApplication.Score = score;
            loanApplication.ScoreExplanation = string.Join(Environment.NewLine,explanation.ToArray());
        }
    }
}