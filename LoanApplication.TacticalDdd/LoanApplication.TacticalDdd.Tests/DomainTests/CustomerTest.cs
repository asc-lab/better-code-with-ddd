using System;
using LoanApplication.TacticalDdd.DomainModel;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.DomainTests
{
    public class CustomerTest
    {
        [Fact]
        public void Customer_Born1974_IsAt2019_45YearsOld()
        {
            var customer = new Customer
            (
                new NationalIdentifier("11111111116"),
                new Name("Jan","B"),
                new DateTime(1974,6,26), 
                new MonetaryAmount(5_000M),
                new Address("Poland","00-001","Warsaw","Zielona 8")
            );

            var ageAt2019 = customer.AgeInYearsAt(new DateTime(2019, 1, 1));
            
            Assert.Equal(45.Years(), ageAt2019);
        }
        
        [Fact]
        public void Customer_Born1974_IsAt2020_46YearsOld()
        {
            var customer = new Customer
            (
                new NationalIdentifier("11111111116"),
                new Name("Jan","B"),
                new DateTime(1974,6,26), 
                new MonetaryAmount(5_000M),
                new Address("Poland","00-001","Warsaw","Zielona 8")
            );

            var ageAt2020 = customer.AgeInYearsAt(new DateTime(2020, 1, 1));
            
            Assert.Equal(46.Years(), ageAt2020);
        }
        
        [Fact]
        public void Customer_Born1974_IsAt2021_47YearsOld()
        {
            var customer = new Customer
            (
                new NationalIdentifier("11111111116"),
                new Name("Jan","B"),
                new DateTime(1974,6,26), 
                new MonetaryAmount(5_000M),
                new Address("Poland","00-001","Warsaw","Zielona 8")
            );

            var ageAt2021 = customer.AgeInYearsAt(new DateTime(2021, 1, 1));
            
            Assert.Equal(47.Years(), ageAt2021);
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Identifier()
        {
            var ex = Assert.Throws<ArgumentException>(() => 
                new Customer
                (
                    null,
                    new Name("Jan","B"),
                    new DateTime(1974,6,26), 
                    new MonetaryAmount(5_000M),
                    new Address("Poland","00-001","Warsaw","Zielona 8")
                )
            );
            
            Assert.Equal("National identifier cannot be null", ex.Message);
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Name()
        {
            var ex = Assert.Throws<ArgumentException>(() => 
                new Customer
                (
                    new NationalIdentifier("11111111116"),
                    null,
                    new DateTime(1974,6,26), 
                    new MonetaryAmount(5_000M),
                    new Address("Poland","00-001","Warsaw","Zielona 8")
                )
            );
            
            Assert.Equal("Name cannot be null", ex.Message);
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Birthdate()
        {
            var ex = Assert.Throws<ArgumentException>(() => 
                new Customer
                (
                    new NationalIdentifier("11111111116"),
                    new Name("Jan","B"),
                    default, 
                    new MonetaryAmount(5_000M),
                    new Address("Poland","00-001","Warsaw","Zielona 8")
                )
            );
            
            Assert.Equal("Birthdate cannot be empty", ex.Message);
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Income()
        {
            var ex = Assert.Throws<ArgumentException>(() => 
                new Customer
                (
                    new NationalIdentifier("11111111116"),
                    new Name("Jan","B"),
                    new DateTime(1974,6,26), 
                    null,
                    new Address("Poland","00-001","Warsaw","Zielona 8")
                )
            );
            
            Assert.Equal("Monthly income cannot be null", ex.Message);
        }
        
        [Fact]
        public void Customer_CannotBeCreatedWithout_Address()
        {
            var ex = Assert.Throws<ArgumentException>(() => 
                new Customer
                (
                    new NationalIdentifier("11111111116"),
                    new Name("Jan","B"),
                    new DateTime(1974,6,26), 
                    new MonetaryAmount(5_000M),
                    null
                )
            );
            
            Assert.Equal("Address cannot be null", ex.Message);
        }
    }
}