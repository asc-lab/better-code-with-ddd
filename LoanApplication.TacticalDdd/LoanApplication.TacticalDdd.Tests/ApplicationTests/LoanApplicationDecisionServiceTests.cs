using System.Collections.Generic;
using System.Security.Claims;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.DomainEvents;
using LoanApplication.TacticalDdd.Tests.Asserts;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;
using static LoanApplication.TacticalDdd.Tests.Builders.LoanApplicationBuilder;
using static LoanApplication.TacticalDdd.Tests.Builders.OperatorBuilder;

namespace LoanApplication.TacticalDdd.Tests.ApplicationTests
{
    public class LoanApplicationDecisionServiceTests
    {
        [Fact]
        public void LoanApplicationDecisionService_GreenApplication_CanBeAccepted()
        {
            var operators = new InMemoryOperatorRepository(new List<Operator>
            {
                GivenOperator().WithLogin("admin").Build()    
            });
            
            var existingApplications = new InMemoryLoanApplicationRepository(new []
            {
                GivenLoanApplication()
                    .WithNumber("123")
                    .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                    .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                    .WithProperty(prop => prop.WithValue(250_000M))
                    .Evaluated()
                    .Build()
            });
            
            var eventBus = new InMemoryBus();
            
            var decisionService = new LoanApplicationDecisionService
            (
                new UnitOfWorkMock(),
                existingApplications,
                operators,
                eventBus
            );
            
            
            decisionService.AcceptApplication("123", OperatorIdentity("admin"));
            
            existingApplications.WithNumber(new LoanApplicationNumber("123"))
                .Should()
                .BeAccepted();
            
            eventBus.Events
                .Should()
                .HaveExpectedNumberOfEvents(1)
                .And.ContainEvent<LoanApplicationAccepted>(e => e.LoanApplicationId==existingApplications.WithNumber(new LoanApplicationNumber("123")).Id.Value);
        }
        
        [Fact]
        public void LoanApplicationDecisionService_GreenApplication_CanBeRejected()
        {
            var operators = new InMemoryOperatorRepository(new List<Operator>
            {
                GivenOperator().WithLogin("admin").Build()    
            });
            
            var existingApplications = new InMemoryLoanApplicationRepository(new []
            {
                GivenLoanApplication()
                    .WithNumber("123")
                    .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                    .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                    .WithProperty(prop => prop.WithValue(250_000M))
                    .Evaluated()
                    .Build()
            });

            var eventBus = new InMemoryBus();
            
            var decisionService = new LoanApplicationDecisionService
            (
                new UnitOfWorkMock(),
                existingApplications,
                operators,
                eventBus
            );
            
            
            decisionService.RejectApplication("123", OperatorIdentity("admin"), null);
            
            existingApplications.WithNumber(new LoanApplicationNumber("123"))
                .Should()
                .BeRejected();

            eventBus.Events
                .Should()
                .HaveExpectedNumberOfEvents(1)
                .And.ContainEvent<LoanApplicationRejected>(e => e.LoanApplicationId==existingApplications.WithNumber(new LoanApplicationNumber("123")).Id.Value);
        }
        
        private ClaimsPrincipal OperatorIdentity(string login)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, login) 
            }));
        }
    }
}