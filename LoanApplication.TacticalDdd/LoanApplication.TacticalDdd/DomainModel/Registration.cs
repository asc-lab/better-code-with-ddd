using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Registration : ValueObject<Registration>
{
    public DateOnly RegistrationDate { get; }
        
    public OperatorId RegisteredBy { get;  }

    public Registration(DateOnly registrationDate, Operator registeredBy)
        : this(registrationDate, registeredBy.Id)
    {
    }

    private Registration(DateOnly registrationDate, OperatorId registeredBy)
    {
        RegistrationDate = registrationDate;
        RegisteredBy = registeredBy;
    }
        
    //To satisfy EF Core
    protected Registration()
    {
    }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return RegistrationDate;
        yield return RegisteredBy;
    }
}