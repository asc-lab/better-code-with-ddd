using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Installer;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.DomainEvents;
using LoanApplication.TacticalDdd.Tests.Asserts;
using LoanApplication.TacticalDdd.Tests.Builders;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.ApplicationTests
{
    public class AcceptApplicationTests
    {
        [Fact]
        public async void AcceptApplication_GreenApplication_CanBeAccepted()
        {
            var operators = new InMemoryOperatorRepository(new List<Operator>
            {
                new OperatorBuilder().WithLogin("admin").Build()    
            });
            
            var existingApplications = new InMemoryLoanApplicationRepository(new []
            {
                new LoanApplicationBuilder()
                    .WithNumber("123")
                    .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                    .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                    .WithProperty(prop => prop.WithValue(250_000M))
                    .Evaluated()
                    .Build()
            });
            
            var eventBus = new InMemoryBus();
            
            var handler = new AcceptLoanApplication.Handler
            (
                new UnitOfWorkMock(),
                existingApplications,
                operators,
                eventBus
            );
            
            
            await handler.Handle
            (
                new AcceptLoanApplication.Command
                {
                    ApplicationNumber = "123",
                    CurrentUser = OperatorIdentity("admin")
                },
                CancellationToken.None
            );
            
            LoanApplicationAssert
                .That(existingApplications.WithNumber("123"))
                .IsInStatus(LoanApplicationStatus.Accepted);
            
            DomainEventsAssert
                .That(eventBus.Events)
                .HasExpectedNumberOfEvents(1)
                .ContainsEvent<LoanApplicationAccepted>(e => e.LoanApplicationId==existingApplications.WithNumber("123").Id);
        }
        
        [Fact]
        public async void RejectApplication_GreenApplication_CanBeRejected()
        {
            var operators = new InMemoryOperatorRepository(new List<Operator>
            {
                new OperatorBuilder().WithLogin("admin").Build()    
            });
            
            var existingApplications = new InMemoryLoanApplicationRepository(new []
            {
                new LoanApplicationBuilder()
                    .WithNumber("123")
                    .WithCustomer(customer => customer.WithAge(25).WithIncome(15_000M))
                    .WithLoan(loan => loan.WithAmount(200_000).WithNumberOfYears(25).WithInterestRate(1.1M))
                    .WithProperty(prop => prop.WithValue(250_000M))
                    .Evaluated()
                    .Build()
            });

            var eventBus = new InMemoryBus();
            
            var handler = new RejectLoanApplication.Handler
            (
                new UnitOfWorkMock(),
                existingApplications,
                operators,
                eventBus
            );


            await handler.Handle
            (
                new RejectLoanApplication.Command
                {
                    ApplicationNumber = "123",
                    CurrentUser = OperatorIdentity("admin")
                },
                CancellationToken.None
            );
            
            LoanApplicationAssert
                .That(existingApplications.WithNumber("123"))
                .IsInStatus(LoanApplicationStatus.Rejected);

            DomainEventsAssert
                .That(eventBus.Events)
                .HasExpectedNumberOfEvents(1)
                .ContainsEvent<LoanApplicationRejected>(e => e.LoanApplicationId==existingApplications.WithNumber("123").Id);
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