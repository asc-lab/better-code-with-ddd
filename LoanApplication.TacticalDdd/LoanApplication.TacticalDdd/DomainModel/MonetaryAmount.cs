using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class MonetaryAmount(decimal amount) : ValueObject<MonetaryAmount>, IComparable<MonetaryAmount>
{
    public static readonly MonetaryAmount Zero = new(0M);

    public decimal Amount { get; } = decimal.Round(amount, 2, MidpointRounding.ToEven);
    
    public MonetaryAmount Add(MonetaryAmount other) => new MonetaryAmount(Amount + other.Amount);

    public MonetaryAmount Subtract(MonetaryAmount other) => new MonetaryAmount(Amount - other.Amount);
        
    public MonetaryAmount MultiplyByPercent(Percent percent) => new MonetaryAmount((this.Amount * percent.Value)/100M);

    public static MonetaryAmount operator +(MonetaryAmount one, MonetaryAmount two) => one.Add(two);
        
    public static MonetaryAmount operator -(MonetaryAmount one, MonetaryAmount two) => one.Subtract(two);
        
    public static MonetaryAmount operator *(MonetaryAmount one, Percent percent) => one.MultiplyByPercent(percent);
        
    public static bool operator >(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)>0;
        
    public static bool operator <(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)<0;
        
    public static bool operator >=(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)>=0;
        
    public static bool operator <=(MonetaryAmount one, MonetaryAmount two) => one.CompareTo(two)<=0;

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Amount;
    }

    public int CompareTo(MonetaryAmount other) => Amount.CompareTo(other.Amount);
}