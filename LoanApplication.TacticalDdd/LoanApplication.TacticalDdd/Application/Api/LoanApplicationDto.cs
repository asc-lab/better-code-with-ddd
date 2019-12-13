using System;

namespace LoanApplication.TacticalDdd.Application.Api
{
    public class LoanApplicationDto
    {
        public string Number { get; set; }
        public string Status { get; set; }
        public string Score { get; set; }
        public string CustomerNationalIdentifier { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime CustomerBirthdate { get; set; }
        public decimal CustomerMonthlyIncome { get; set; }
        public AddressDto CustomerAddress { get; set; }
        public decimal PropertyValue { get; set; }
        public AddressDto PropertyAddress { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanNumberOfYears { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string DecisionBy { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}