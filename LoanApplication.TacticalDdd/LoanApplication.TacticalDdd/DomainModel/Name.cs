using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Name : ValueObject<Name>
{
    public string First { get; }
    public string Last { get; }
    
    public Name(string first, string last)
    {
        if (string.IsNullOrWhiteSpace(first))
            throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(last))
            throw new ArgumentException("First name cannot be empty");

        First = first;
        Last = last;
    }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return First;
        yield return Last;
    }
}