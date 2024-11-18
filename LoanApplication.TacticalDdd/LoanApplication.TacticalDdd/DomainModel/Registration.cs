using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Registration : ValueObject<Registration>
{
    public DateTime RegistrationDate { get; }

    public Guid RegisteredBy { get; }
    
    public Registration(DateTime registrationDate, Operator registeredBy)
        : this(registrationDate, registeredBy.Id)
    {
    }

    [JsonConstructor]
    public Registration(DateTime registrationDate, Guid registeredBy)
    {
        RegistrationDate = registrationDate;
        RegisteredBy = registeredBy;
    }

    
    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return RegistrationDate;
        yield return RegisteredBy;
    }
}