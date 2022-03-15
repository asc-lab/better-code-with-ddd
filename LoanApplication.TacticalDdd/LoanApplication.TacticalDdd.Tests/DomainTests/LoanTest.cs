using FluentAssertions;
using LoanApplication.TacticalDdd.DomainModel;
using static LoanApplication.TacticalDdd.Tests.Builders.LoanBuilder;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.DomainTests;

public class LoanTest
{
    [Fact]
    public void Can_calculate_monthly_installment()
    {
        var loan = GivenLoan()
            .WithAmount(420_000M)
            .WithNumberOfYears(3)
            .WithInterestRate(5M)
            .Build();

        var installment = loan.MonthlyInstallment(); 
            
        installment.Should().Be(new MonetaryAmount(12_587.78M));
    }
}