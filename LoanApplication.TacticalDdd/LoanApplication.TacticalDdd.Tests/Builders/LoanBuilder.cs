using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Builders;

public class LoanBuilder
{
    private MonetaryAmount amount = new(200_000M);
    private Percent interestRate = 1.Percent();
    private int numberOfYears = 20;

    public LoanBuilder WithAmount(decimal loanAmount)
    {
        amount = new MonetaryAmount(loanAmount);
        return this;
    }

    public LoanBuilder WithNumberOfYears(int numOfYears)
    {
        numberOfYears = numOfYears;
        return this;
    }

    public LoanBuilder WithInterestRate(decimal rate)
    {
        interestRate = new Percent(rate);
        return this;
    }

    public Loan Build()
    {
        return new Loan(amount, numberOfYears, interestRate);
    }
}