using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Builders
{
    public class OperatorBuilder
    {
        private string login = "admin";
        private decimal competenceLevel = 1_000_000M;

        public OperatorBuilder WithLogin(string login)
        {
            this.login = login;
            return this;
        }
        
        public OperatorBuilder WithCompetenceLevel(decimal level)
        {
            this.competenceLevel = level;
            return this;
        }

        public Operator Build()
        {
            return new Operator(login,login,login,login,new MonetaryAmount(competenceLevel));
        }
    }
}