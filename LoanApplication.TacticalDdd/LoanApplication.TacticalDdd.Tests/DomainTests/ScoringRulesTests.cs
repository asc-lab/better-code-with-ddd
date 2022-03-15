using FluentAssertions;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;
using static LoanApplication.TacticalDdd.Tests.Builders.LoanApplicationBuilder;

namespace LoanApplication.TacticalDdd.Tests.DomainTests;

public class ScoringRulesTests
{
    private readonly ScoringRulesFactory scoringRulesFactory = new ScoringRulesFactory(new DebtorRegistryMock());
        
    [Fact]
    public void PropertyValueHigherThanLoan_LoanAmountMustBeLowerThanPropertyValue_IsSatisfied()
    {
        var application = GivenLoanApplication()
            .WithProperty(prop => prop.WithValue(750_000M))
            .WithLoan(loan => loan.WithAmount(300_000M))
            .Build();
            
        var rule = new LoanAmountMustBeLowerThanPropertyValue();
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeTrue();
    }
        
    [Fact]
    public void PropertyValueLowerThanLoan_LoanAmountMustBeLowerThanPropertyValue_IsNotSatisfied()
    {
        var application = GivenLoanApplication()
            .WithProperty(prop => prop.WithValue(750_000M))
            .WithLoan(loan => loan.WithAmount(800_000M))
            .Build();
            
        var rule = new LoanAmountMustBeLowerThanPropertyValue();
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeFalse();
    }

    [Fact]
    public void CustomerNotOlderThan65AtEndOfLoan_CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65_IsSatisfied()
    {
        var application = GivenLoanApplication()
            .WithLoan(loan => loan.WithNumberOfYears(20))
            .WithCustomer(customer => customer.WithAge(26))
            .Build();
            
        var rule = new CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65();
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeTrue();
    }
        
    [Fact]
    public void CustomerOlderThan65AtEndOfLoan_CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65_IsNotSatisfied()
    {
        var application = GivenLoanApplication()
            .WithLoan(loan => loan.WithNumberOfYears(20))
            .WithCustomer(customer => customer.WithAge(46))
            .Build();
            
        var rule = new CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65();
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeFalse();
    }

    [Fact]
    public void CustomerIncome15PercentHigherThenOfInstallment_InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome_IsSatisfied()
    {
        var application = GivenLoanApplication()
            .WithLoan(loan => loan.WithNumberOfYears(25).WithAmount(400_000M).WithInterestRate(1M))
            .WithCustomer(customer => customer.WithIncome(11_000M))
            .Build();
            
        var rule = new InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome();
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeTrue();
    }
        
    [Fact]
    public void CustomerIncome15PercentLowerThenOfInstallment_InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome_IsNotSatisfied()
    {
        var application = GivenLoanApplication()
            .WithLoan(loan => loan.WithNumberOfYears(20).WithAmount(400_000M).WithInterestRate(1M))
            .WithCustomer(customer => customer.WithIncome(4_000M))
            .Build();
            
        var rule = new InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome();
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeFalse();
    }

    [Fact]
    public void CustomerIsNotARegisteredDebtor_CustomerNotADebtor_IsSatisfied()
    {
        var application = GivenLoanApplication()
            .WithCustomer(customer => customer.WithIdentifier("71041864667"))
            .Build();
            
        var rule = new CustomerIsNotARegisteredDebtor(new DebtorRegistryMock());
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeTrue();
    }
        
    [Fact]
    public void CustomerIsNotARegisteredDebtor_CustomerNotADebtor_IsNotSatisfied()
    {
        var application = GivenLoanApplication()
            .WithCustomer(customer => customer.WithIdentifier(DebtorRegistryMock.DebtorNationalIdentifier))
            .Build();
            
        var rule = new CustomerIsNotARegisteredDebtor(new DebtorRegistryMock());
        var ruleCheckResult = rule.IsSatisfiedBy(application);
            
        ruleCheckResult.Should().BeFalse();
    }
        
    [Fact]
    public void WhenAnyRuleIsNotSatisfied_ScoringResult_IsRed()
    {
        var application = GivenLoanApplication()
            .WithLoan(loan => loan.WithNumberOfYears(20).WithAmount(400_000M).WithInterestRate(1M))
            .WithCustomer(customer => customer.WithIncome(4_000M))
            .Build();

        var score = scoringRulesFactory.DefaultSet.Evaluate(application);
            
        score.Score.Should().Be(ApplicationScore.Red);
    }

    [Fact]
    public void WhenAllRulesAreSatisfied_ScoringResult_IsGreen()
    {
        var application = GivenLoanApplication()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Build();

        var score = scoringRulesFactory.DefaultSet.Evaluate(application);
            
        score.Score.Should().Be(ApplicationScore.Green);    
    }
}