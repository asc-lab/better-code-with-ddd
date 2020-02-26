namespace LoanApplication.TacticalDdd.DomainModel
{
    public interface IOperatorRepository
    {
        void Add(Operator @operator);

        Operator WithLogin(Login login);
    }
}