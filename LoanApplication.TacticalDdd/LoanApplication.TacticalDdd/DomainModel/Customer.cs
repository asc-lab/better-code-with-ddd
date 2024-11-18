using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Customer : ValueObject<Customer>
{
    public NationalIdentifier NationalIdentifier { get;  }
    public Name Name { get; }
    public DateOnly Birthdate { get; }
    public MonetaryAmount MonthlyIncome { get; }
    public Address Address { get;  }

    public Customer
    (
        NationalIdentifier nationalIdentifier, 
        Name name, 
        DateOnly birthdate, 
        MonetaryAmount monthlyIncome, 
        Address address
    )
    {
        if (nationalIdentifier==null)
            throw new ArgumentException("National identifier cannot be null");
        if (name==null)
            throw new ArgumentException("Name cannot be null");
        if (monthlyIncome==null)
            throw new ArgumentException("Monthly income cannot be null");
        if (address==null)
            throw new ArgumentException("Address cannot be null");
        if (birthdate==default)
            throw new ArgumentException("Birthdate cannot be empty");
            
        NationalIdentifier = nationalIdentifier;
        Name = name;
        Birthdate = birthdate;
        MonthlyIncome = monthlyIncome;
        Address = address;
    }

    public AgeInYears AgeInYearsAt(DateOnly date) => AgeInYears.Between(Birthdate, date);

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        return new List<object>
        {
            NationalIdentifier,
            Name,
            Birthdate,
            MonthlyIncome,
            Address
        };
    }
}