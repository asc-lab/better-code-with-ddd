using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Property : ValueObject<Property>
{
    public MonetaryAmount Value { get; }
    public Address Address { get; }

    public Property(MonetaryAmount value, Address address)
    {
        if (value==null)
            throw new ArgumentException("Value cannot be null");
        if (address==null)
            throw new ArgumentException("Address cannot be null");
        if (value <= MonetaryAmount.Zero)
            throw new ArgumentException("Property value must be higher than 0");
            
        Value = value;
        Address = address;
    }
        
    //To satisfy EF Core
    protected Property()
    {
    }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck() => new List<object> {Value, Address};
    
}