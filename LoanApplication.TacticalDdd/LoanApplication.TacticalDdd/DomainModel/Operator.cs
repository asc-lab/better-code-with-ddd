using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Operator : Entity
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public MonetaryAmount CompetenceLevel { get; private set; }

        public Operator(string login, string password, string firstName, string lastName, MonetaryAmount competenceLevel)
        {
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            CompetenceLevel = competenceLevel;
        }

        public bool CanAccept(MonetaryAmount loanLoanAmount) => loanLoanAmount <= CompetenceLevel;

    }
}