using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.Tests.Builders;
using LoanApplication.TacticalDdd.Tests.Mocks;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.ApplicationTests
{
    public class SubmitLoanApplicationTests
    {
        [Fact]
        public async Task SubmitLoanApplication_ValidApplication_GetsSubmitted()
        {
            var operators = new InMemoryOperatorRepository(new List<Operator>
            {
                new OperatorBuilder().WithLogin("admin").Build()    
            });
            
            var existingApplications = new InMemoryLoanApplicationRepository(new List<DomainModel.LoanApplication>());
            
            var handler = new SubmitLoanApplication.Handler
            (
                new UnitOfWorkMock(), 
                existingApplications,
                operators
            );

            var validApplication = new LoanApplicationDto
            {
                CustomerNationalIdentifier = "11111111119",
                CustomerFirstName = "Frank",
                CustomerLastName = "Oz",
                CustomerBirthdate = SysTime.Now().AddYears(-25),
                CustomerMonthlyIncome = 10_000M,
                CustomerAddress = new AddressDto
                {
                    Country = "PL",
                    City = "Warsaw",
                    Street = "Chłodna 52",
                    ZipCode = "00-121"
                },
                PropertyValue = 320_000M,
                PropertyAddress = new AddressDto
                {
                    Country = "PL",
                    City = "Warsaw",
                    Street = "Wilcza 10",
                    ZipCode = "00-421"
                },
                LoanAmount = 100_000M,
                LoanNumberOfYears = 25,
                InterestRate = 1.1M
            };

            var newApplicationNumber = await handler.Handle
            (
                new SubmitLoanApplication.Command
                {
                    LoanApplication = validApplication,
                    CurrentUser = OperatorIdentity("admin")
                },
                CancellationToken.None
            );

            Assert.False(string.IsNullOrWhiteSpace(newApplicationNumber));
            var createdLoanApplication = existingApplications.WithNumber(newApplicationNumber);
            Assert.NotNull(createdLoanApplication);
        }
        
        
        [Fact]
        public async Task SubmitLoanApplication_InvalidApplication_IsNotSaved()
        {
            var operators = new InMemoryOperatorRepository(new List<Operator>
            {
                new OperatorBuilder().WithLogin("admin").Build()    
            });
            
            var existingApplications = new InMemoryLoanApplicationRepository(new List<DomainModel.LoanApplication>());
            
            var handler = new SubmitLoanApplication.Handler
            (
                new UnitOfWorkMock(), 
                existingApplications,
                operators
            );

            var validApplication = new LoanApplicationDto
            {
                CustomerNationalIdentifier = "11111111119111",
                CustomerFirstName = "Frank",
                CustomerLastName = "Oz",
                CustomerBirthdate = SysTime.Now().AddYears(-25),
                CustomerMonthlyIncome = 10_000M,
                CustomerAddress = new AddressDto
                {
                    Country = "PL",
                    City = "Warsaw",
                    Street = "Chłodna 52",
                    ZipCode = "00-121"
                },
                PropertyValue = 320_000M,
                PropertyAddress = new AddressDto
                {
                    Country = "PL",
                    City = "Warsaw",
                    Street = "Wilcza 10",
                    ZipCode = "00-421"
                },
                LoanAmount = 100_000M,
                LoanNumberOfYears = 25,
                InterestRate = 1.1M
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle
            (
                new SubmitLoanApplication.Command
                {
                    LoanApplication = validApplication,
                    CurrentUser =  OperatorIdentity("admin")
                },
                CancellationToken.None
            ));
            
            Assert.Equal("National Identifier must be 11 chars long", ex.Message);
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