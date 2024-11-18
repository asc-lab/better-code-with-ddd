namespace LoanApplication.TacticalDdd.DomainModel.Ddd;

public abstract class ValueObject<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

    public override bool Equals(object other)
    {
        return Equals(other as T);
    }

    public virtual bool Equals(T other)
    {
        if (other == null) return false;
        return GetAttributesToIncludeInEqualityCheck().SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
    }

    public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        var hash = 17;
        foreach (var obj in GetAttributesToIncludeInEqualityCheck())
            hash = hash * 31 + (obj == null ? 0 : obj.GetHashCode());

        return hash;
    }
}