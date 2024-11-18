using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.Tests.Asserts;
using LoanApplication.TacticalDdd.Tests.Builders;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.DomainTests;

public class LoanApplicationTest
{
    private readonly ScoringRulesFactory scoringRulesFactory = new(new DebtorRegistryMock());

    [Fact]
    public void NewApplication_IsCreatedIn_NewStatus_AndNullScore()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Build();

        LoanApplicationAssert.That(application)
            .IsInStatus(LoanApplicationStatus.New)
            .ScoreIsNull();
    }

    [Fact]
    public void ValidApplication_EvaluationScore_IsGreen()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Build();

        application.Evaluate(scoringRulesFactory.DefaultSet);

        LoanApplicationAssert.That(application)
            .IsInStatus(LoanApplicationStatus.New)
            .ScoreIs(ApplicationScore.Green);
    }

    [Fact]
    public void InvalidApplication_EvaluationScore_IsRed_And_StatusIsRejected()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(55).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Build();

        application.Evaluate(scoringRulesFactory.DefaultSet);

        LoanApplicationAssert.That(application)
            .IsInStatus(LoanApplicationStatus.Rejected)
            .ScoreIs(ApplicationScore.Red);
    }

    [Fact]
    public void LoanApplication_InStatusNew_EvaluatedGreen_OperatorHasCompetenceLevel_CanBeAccepted()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Evaluated()
            .Build();

        var user = new OperatorBuilder()
            .WithLogin("admin")
            .WithCompetenceLevel(1_000_000M)
            .Build();

        application.Accept(user);

        LoanApplicationAssert.That(application)
            .IsInStatus(LoanApplicationStatus.Accepted)
            .ScoreIs(ApplicationScore.Green);
    }

    [Fact]
    public void LoanApplication_InStatusNew_EvaluatedGreen_OperatorDoesNotHaveCompetenceLevel_CannotBeAccepted()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Evaluated()
            .Build();

        var user = new OperatorBuilder()
            .WithLogin("admin")
            .WithCompetenceLevel(100_000M)
            .Build();

        var ex = Assert.Throws<ApplicationException>(() => application.Accept(user));
        Assert.Equal("Operator does not have required competence level to accept application", ex.Message);
    }

    [Fact]
    public void LoanApplication_InStatusNew_EvaluatedGreen_CanBeRejected()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Evaluated()
            .Build();

        var user = new OperatorBuilder().WithLogin("admin").Build();
        application.Reject(user);

        LoanApplicationAssert.That(application)
            .IsInStatus(LoanApplicationStatus.Rejected)
            .ScoreIs(ApplicationScore.Green);
    }

    [Fact]
    public void LoanApplication_WithoutScore_CannotBeAccepted()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .NotEvaluated()
            .Build();

        var user = new OperatorBuilder().WithLogin("admin").Build();
        var ex = Assert.Throws<ApplicationException>(() => application.Accept(user));
        Assert.Equal("Cannot accept application before scoring", ex.Message);
    }

    [Fact]
    public void LoanApplication_WithoutScore_CanBeRejected()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .NotEvaluated()
            .Build();

        var user = new OperatorBuilder().WithLogin("admin").Build();
        application.Reject(user);

        LoanApplicationAssert.That(application)
            .IsInStatus(LoanApplicationStatus.Rejected)
            .ScoreIsNull();
    }

    [Fact]
    public void LoanApplication_Accepted_CannotBeRejected()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Evaluated()
            .Accepted()
            .Build();

        var user = new OperatorBuilder().WithLogin("admin").Build();
        var ex = Assert.Throws<ApplicationException>(() => application.Reject(user));
        Assert.Equal("Cannot reject application that is already accepted or rejected", ex.Message);
    }

    [Fact]
    public void LoanApplication_Rejected_CannotBeAccepted()
    {
        var application = new LoanApplicationBuilder()
            .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
            .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
            .WithProperty(prop => prop.WithValue(250_000M))
            .Evaluated()
            .Rejected()
            .Build();

        var user = new OperatorBuilder().WithLogin("admin").Build();
        var ex = Assert.Throws<ApplicationException>(() => application.Accept(user));
        Assert.Equal("Cannot accept application that is already accepted or rejected", ex.Message);
    }
}