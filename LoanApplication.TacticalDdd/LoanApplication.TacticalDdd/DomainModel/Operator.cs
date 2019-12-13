using System;
using System.Collections.Generic;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Operator : Entity<OperatorId>
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public MonetaryAmount CompetenceLevel { get; private set; }

        public Operator(string login, string password, string firstName, string lastName, MonetaryAmount competenceLevel)
        {
            Id = new OperatorId(Guid.NewGuid());
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            CompetenceLevel = competenceLevel;
        }
        
        //To satisfy EF Core
        protected Operator()
        {
        }

        public bool CanAccept(MonetaryAmount loanLoanAmount) => loanLoanAmount <= CompetenceLevel;

    }

    public class OperatorId : ValueObject<OperatorId>
    {
        public Guid Value { get; }

        public OperatorId(Guid value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return Value;
        }
        
        protected OperatorId()
        {
        }
    }
}