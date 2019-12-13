using System;
using System.Collections.Generic;
using System.ComponentModel;
using LoanApplication.Infrastructure.Common;

namespace LoanApplication.BusinessLogic
{
    public class LoanApplication : Entity
    {
        public string Number { get; set; }
        public LoanApplicationStatus Status { get; set; }
        public ApplicationScore? Score { get; set; }
        
        public string ScoreExplanation { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Property Property { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanNumberOfYears { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime? DecisionDate { get; set; }
        public virtual Operator DecisionBy { get; set; }
        public virtual Operator RegisteredBy { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}