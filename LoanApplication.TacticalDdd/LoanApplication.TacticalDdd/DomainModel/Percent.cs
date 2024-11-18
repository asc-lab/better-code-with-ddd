using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Percent : ValueObject<Percent>, IComparable<Percent>
{
    public static readonly Percent Zero = new(0M);

    public Percent(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Percent value cannot be negative");

        Value = value;
    }

    public decimal Value { get; }

    public static bool operator >(Percent one, Percent two) => one.CompareTo(two)>0;
        
    public static bool operator <(Percent one, Percent two) => one.CompareTo(two)<0;
        
    public static bool operator >=(Percent one, Percent two) => one.CompareTo(two)>=0;
        
    public static bool operator <=(Percent one, Percent two) => one.CompareTo(two)<=0;

    public int CompareTo(Percent other) => Value.CompareTo(other.Value);
        
    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Value;
    }
}

public static class PercentExtensions
{
    public static Percent Percent(this int value)
    {
        return new Percent(value);
    }

    public static Percent Percent(this decimal value)
    {
        return new Percent(value);
    }
}