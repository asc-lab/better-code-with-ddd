using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class NationalIdentifier : ValueObject<NationalIdentifier>
{
    public string Value { get; }
    
    public NationalIdentifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("National Identifier cannot be null or empty string");

        if (value.Length != 11)
            throw new ArgumentException("National Identifier must be 11 chars long");

        Value = value;
    }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Value;
    }
}