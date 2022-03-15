using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Name : ValueObject<Name>
{
    public Name(string first, string last)
    {
        if (string.IsNullOrWhiteSpace(first))
            throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(last))
            throw new ArgumentException("Last name cannot be empty");
            
        First = first;
        Last = last;
    }

    //To satisfy EF Core
    protected Name()
    {
    }

    public string First { get;  set; }
    public string Last { get;  set; }
        
    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return First;
        yield return Last;
    }
}