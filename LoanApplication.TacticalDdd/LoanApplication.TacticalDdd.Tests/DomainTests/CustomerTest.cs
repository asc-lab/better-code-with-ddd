using System;
using FluentAssertions;
using LoanApplication.TacticalDdd.DomainModel;
using Xunit;
using static LoanApplication.TacticalDdd.Tests.Builders.CustomerBuilder; 

namespace LoanApplication.TacticalDdd.Tests.DomainTests
{
    public class CustomerTest
    {
        [Fact]
        public void Customer_Born1974_IsAt2019_45YearsOld()
        {
            var customer =  GivenCustomer()
                .BornOn(new DateTime(1974, 6, 26))
                .Build();

            var ageAt2019 = customer.AgeInYearsAt(new DateTime(2019, 1, 1));
            
            ageAt2019.Should().Be(45.Years());
        }
        
        [Fact]
        public void Customer_Born1974_IsAt2020_46YearsOld()
        {
            var customer = GivenCustomer()
                .BornOn(new DateTime(1974, 6, 26))
                .Build();
            

            var ageAt2020 = customer.AgeInYearsAt(new DateTime(2020, 1, 1));
            
            ageAt2020.Should().Be(46.Years());
        }
        
        [Fact]
        public void Customer_Born1974_IsAt2021_47YearsOld()
        {
            var customer =  GivenCustomer()
                .BornOn(new DateTime(1974, 6, 26))
                .Build();


            var ageAt2021 = customer.AgeInYearsAt(new DateTime(2021, 1, 1));
            
            ageAt2021.Should().Be(47.Years());
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Identifier()
        {
            Action act = () => new Customer
            (
                null,
                new Name("Jan", "B"),
                new DateTime(1974, 6, 26),
                new MonetaryAmount(5_000M),
                new Address("Poland", "00-001", "Warsaw", "Zielona 8")
            );

            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("National identifier cannot be null");
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Name()
        {
            Action act = () => new Customer
            (
                new NationalIdentifier("11111111116"),
                null,
                new DateTime(1974, 6, 26),
                new MonetaryAmount(5_000M),
                new Address("Poland", "00-001", "Warsaw", "Zielona 8")
            );
            
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Name cannot be null");
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Birthdate()
        {
            Action act = () => new Customer
            (
                new NationalIdentifier("11111111116"),
                new Name("Jan","B"),
                default, 
                new MonetaryAmount(5_000M),
                new Address("Poland","00-001","Warsaw","Zielona 8")
            );
            
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Birthdate cannot be empty");
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Income()
        {
            Action act = () => new Customer
            (
                new NationalIdentifier("11111111116"),
                new Name("Jan","B"),
                new DateTime(1974,6,26), 
                null,
                new Address("Poland","00-001","Warsaw","Zielona 8")
            );
            
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Monthly income cannot be null");
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Address()
        {
            Action act = () => new Customer
            (
                new NationalIdentifier("11111111116"),
                new Name("Jan","B"),
                new DateTime(1974,6,26), 
                new MonetaryAmount(5_000M),
                null
            );
            
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Address cannot be null");
        }
    }
}