using System;

namespace LoanApplication.TacticalDdd.Application.Api
{
    public class LoanApplicationInfoDto
    {
        public string Number { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DecisionDate { get; set; }
        public decimal LoanAmount { get; set; }
        public string DecisionBy { get; set; }
    }
}