using System;
using System.Collections.Generic;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Percent : ValueObject<Percent>,  IComparable<Percent>
    {
        public decimal Value { get; }
        
        public static readonly Percent Zero = new Percent(0M);

        public Percent(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("Percent value cannot be negative");

            Value = value;
        }
        
        //To satisfy EF Core
        protected Percent()
        {
        }
        
        public static bool operator >(Percent one, Percent two) => one.CompareTo(two)>0;
        
        public static bool operator <(Percent one, Percent two) => one.CompareTo(two)<0;
        
        public static bool operator >=(Percent one, Percent two) => one.CompareTo(two)>=0;
        
        public static bool operator <=(Percent one, Percent two) => one.CompareTo(two)<=0;

        public int CompareTo(Percent other)
        {
            return Value.CompareTo(other.Value);
        }
        
        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return Value;
        }
    }

    public static class PercentExtensions
    {
        public static Percent Percent(this int value) => new Percent(value);
        
        public static Percent Percent(this decimal value) => new Percent(value);
    }
}