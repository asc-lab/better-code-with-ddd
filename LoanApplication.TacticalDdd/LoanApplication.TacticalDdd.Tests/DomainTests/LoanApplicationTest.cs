using System;
using FluentAssertions;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.Tests.Asserts;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;
using static LoanApplication.TacticalDdd.Tests.Builders.LoanApplicationBuilder;
using static LoanApplication.TacticalDdd.Tests.Builders.OperatorBuilder;

namespace LoanApplication.TacticalDdd.Tests.DomainTests
{
    public class LoanApplicationTest
    {
        private readonly ScoringRulesFactory scoringRulesFactory = new ScoringRulesFactory(new DebtorRegistryMock());
        
        [Fact]
        public void NewApplication_IsCreatedIn_NewStatus_AndNullScore()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Build();

            application
                .Should()
                .BeInStatus(LoanApplicationStatus.New)
                .And
                .ScoreIsNull();
        }

        [Fact]
        public void ValidApplication_EvaluationScore_IsGreen()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Build();
            
            application.Evaluate(scoringRulesFactory.DefaultSet);

            application
                .Should()
                .BeInStatus(LoanApplicationStatus.New)
                .And
                .ScoreIs(ApplicationScore.Green);
        }
        
        [Fact]
        public void InvalidApplication_EvaluationScore_IsRed_And_StatusIsRejected()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(55).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Build();
            
            application.Evaluate(scoringRulesFactory.DefaultSet);

            application
                .Should()
                .BeInStatus(LoanApplicationStatus.Rejected)
                .And.ScoreIs(ApplicationScore.Red);
        }

        [Fact]
        public void LoanApplication_InStatusNew_EvaluatedGreen_OperatorHasCompetenceLevel_CanBeAccepted()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Evaluated()
                .Build();
            
            var user = GivenOperator()
                .WithCompetenceLevel(1_000_000M)
                .Build();
            
            application.Accept(user);

            application
                .Should()
                .BeInStatus(LoanApplicationStatus.Accepted)
                .And.ScoreIs(ApplicationScore.Green);
        }
        
        [Fact]
        public void LoanApplication_InStatusNew_EvaluatedGreen_OperatorDoesNotHaveCompetenceLevel_CannotBeAccepted()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Evaluated()
                .Build();
            
            var user = GivenOperator()
                .WithCompetenceLevel(100_000M)
                .Build();

            Action act = () => application.Accept(user);
            
            act
                .Should()
                .Throw<ApplicationException>()
                .WithMessage("Operator does not have required competence level to accept application");
        }
        
        [Fact]
        public void LoanApplication_InStatusNew_EvaluatedGreen_CanBeRejected()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Evaluated()
                .Build();
            
            var user = GivenOperator().Build();
            application.Reject(user);

            application
                .Should()
                .BeInStatus(LoanApplicationStatus.Rejected)
                .And.ScoreIs(ApplicationScore.Green);
        }

        [Fact]
        public void LoanApplication_WithoutScore_CannotBeAccepted()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .NotEvaluated()
                .Build();
            
            var user = GivenOperator().Build();

            Action act = () => application.Accept(user);
            
            act
                .Should()
                .Throw<ApplicationException>()
                .WithMessage("Cannot accept application before scoring");
        }
        
        [Fact]
        public void LoanApplication_WithoutScore_CanBeRejected()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .NotEvaluated()
                .Build();
            
            var user = GivenOperator().Build();
            application.Reject(user);

            application
                .Should()
                .BeInStatus(LoanApplicationStatus.Rejected)
                .And.ScoreIsNull();
        }

        [Fact]
        public void LoanApplication_Accepted_CannotBeRejected()
        {
            var application = GivenLoanApplication()
                .Evaluated()
                .Accepted()
                .Build();
            
            var user = GivenOperator().Build();

            Action act = () => application.Reject(user);
            
            act
                .Should()
                .Throw<ApplicationException>()
                .WithMessage("Cannot reject application that is already accepted or rejected");
        }
        
        [Fact]
        public void LoanApplication_Rejected_CannotBeAccepted()
        {
            var application = GivenLoanApplication()
                .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                .WithProperty(prop => prop.WithValue(250_000M))
                .Evaluated()
                .Rejected()
                .Build();
            
            var user = GivenOperator().Build();

            Action act = () => application.Accept(user);
            
            act
                .Should()
                .Throw<ApplicationException>()
                .WithMessage("Cannot accept application that is already accepted or rejected");
        }
    }
}