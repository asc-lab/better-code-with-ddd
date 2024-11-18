using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.Tests.Asserts;
using LoanApplication.TacticalDdd.Tests.Builders;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.ApplicationTests;

public class EvaluateLoanApplicationTests
{
    [Fact]
    public async void EvaluateLoanApplication_ApplicationThatSatisfiesAllRules_IsEvaluatedGreen()
    {
        var existingApplications = new InMemoryLoanApplicationRepository(new[]
        {
            new LoanApplicationBuilder()
                .WithNumber("123")
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Build()
        });

        var handler = new EvaluateLoanApplication.Handler
        (
            new UnitOfWorkMock(),
            existingApplications,
            new DebtorRegistryMock()
        );

        await handler.Handle
        (
            new EvaluateLoanApplication.Command { ApplicationNumber = "123" },
            CancellationToken.None
        );

        LoanApplicationAssert
            .That(existingApplications.WithNumber("123"))
            .ScoreIs(ApplicationScore.Green);
    }

    [Fact]
    public async void EvaluateLoanApplication_ApplicationThatDoesNotSatisfyAllRules_IsEvaluatedRedAndRejected()
    {
        var existingApplications = new InMemoryLoanApplicationRepository(new[]
        {
            new LoanApplicationBuilder()
                .WithNumber("123")
                .WithCustomer(customer => customer.WithAge(55).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Build()
        });

        var handler = new EvaluateLoanApplication.Handler
        (
            new UnitOfWorkMock(),
            existingApplications,
            new DebtorRegistryMock()
        );

        await handler.Handle
        (
            new EvaluateLoanApplication.Command { ApplicationNumber = "123" },
            CancellationToken.None
        );

        LoanApplicationAssert
            .That(existingApplications.WithNumber("123"))
            .ScoreIs(ApplicationScore.Red)
            .IsInStatus(LoanApplicationStatus.Rejected);
    }
}