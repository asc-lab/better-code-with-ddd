using System;
using System.Collections.Generic;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Operator : Entity<OperatorId>
    {
        public Login Login { get; private set; }
        public Password Password { get; private set; }
        public Name Name { get; private set; }
        public MonetaryAmount CompetenceLevel { get; private set; }

        public Operator(Login login, Password password, Name name, MonetaryAmount competenceLevel)
        {
            Id = new OperatorId(Guid.NewGuid());
            Login = login;
            Password = password;
            Name = name;
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
    
    
    public class Login : ValueObject<Login>
    {
        public string Value { get; }
        public Login(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Login cannot be null or empty string");
            Value = value;
        }
        
        
        public static Login Of(string login) => new Login(login);

        public static implicit operator string(Login login) => login.Value;
        
        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return Value;
        }
    }
    
    public class Password : ValueObject<Password>
    {
        public string Value { get; }
        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Password cannot be null or empty string");
            Value = value;
        }
        
        
        public static Password Of(string value) => new Password(value);

        public static implicit operator string(Password password) => password.Value;
        
        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return Value;
        }
    }
}