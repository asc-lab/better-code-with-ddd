using System;
using LoanApplication.Infrastructure.Common;

namespace LoanApplication.BusinessLogic
{
    public class Customer : Entity
    {
        public string NationalIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public decimal MonthlyIncome { get; set; }
        public virtual Address Address { get; set; }

    }
}