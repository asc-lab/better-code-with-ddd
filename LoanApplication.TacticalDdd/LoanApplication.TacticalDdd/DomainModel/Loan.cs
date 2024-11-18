using LoanApplication.TacticalDdd.DomainModel.Ddd;
using static System.Math;
using static System.Linq.Enumerable;

namespace LoanApplication.TacticalDdd.DomainModel;

public class Loan : ValueObject<Loan>
{
    public Loan(MonetaryAmount loanAmount, int loanNumberOfYears, Percent interestRate)
    {
        if (loanAmount == null)
            throw new ArgumentException("Loan amount cannot be null");
        if (interestRate == null)
            throw new ArgumentException("Interest rate cannot be null");
        if (loanAmount <= MonetaryAmount.Zero)
            throw new ArgumentException("Loan amount must be grated than 0");
        if (interestRate <= Percent.Zero)
            throw new ArgumentException("Interest rate must be higher than 0");
        if (loanNumberOfYears <= 0)
            throw new ArgumentException("Loan number of years must be greater than 0");

        LoanAmount = loanAmount;
        LoanNumberOfYears = loanNumberOfYears;
        InterestRate = interestRate;
    }

    public MonetaryAmount LoanAmount { get; }
    public int LoanNumberOfYears { get; }
    public Percent InterestRate { get; }

    public MonetaryAmount MonthlyInstallment()
    {
        var totalInstallments = LoanNumberOfYears * 12;

        var x = Range(1, totalInstallments).Sum(
            i => Pow(1.0 + (double)InterestRate.Value / 100 / 12, -1 * i));

        return new MonetaryAmount(LoanAmount.Amount / Convert.ToDecimal(x));
    }

    public DateOnly LastInstallmentsDate() => SysTime.CurrentDate().AddYears(LoanNumberOfYears);

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        return new List<object>
        {
            LoanAmount,
            LoanNumberOfYears,
            InterestRate
        };
    }
}