namespace LoanApplication.Contracts
{
    public class LoanApplicationSearchCriteriaDto
    {
        public string ApplicationNumber { get; set; }
        public string CustomerNationalIdentifier { get; set; }
        public string DecisionBy { get; set; }
        public string RegisteredBy { get; set; }
    }
}