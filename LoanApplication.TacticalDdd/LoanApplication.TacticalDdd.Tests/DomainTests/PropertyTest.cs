using FluentAssertions;
using LoanApplication.TacticalDdd.DomainModel;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.DomainTests;

public class PropertyTest
{
    [Fact]
    public void PropertiesWithTheSameValueAndAddress_AreEqual()
    {
        var propOne = new Property
        (
            new MonetaryAmount(100000),
            new Address
            (
                "PL",
                "01-001",
                "Warsaw",
                "Zielona 7"
            )
        );
        var propTwo = new Property
        (
            new MonetaryAmount(100000),
            new Address
            (
                "PL",
                "01-001",
                "Warsaw",
                "Zielona 7"
            )
        );
            
        propOne.Equals(propTwo).Should().BeTrue();
    }
        
    [Fact]
    public void PropertiesWithDifferentValueAndTheSameAddress_AreNotEqual()
    {
        var propOne = new Property
        (
            new MonetaryAmount(100000),
            new Address
            (
                "PL",
                "01-001",
                "Warsaw",
                "Zielona 7"
            )
        );
        var propTwo = new Property
        (
            new MonetaryAmount(100001),
            new Address
            (
                "PL",
                "01-001",
                "Warsaw",
                "Zielona 7"
            )
        );
            
        propOne.Equals(propTwo).Should().BeFalse();
    }
        
    [Fact]
    public void PropertiesWithTheSameValueAndDifferentAddress_AreNotEqual()
    {
        var propOne = new Property
        (
            new MonetaryAmount(100000),
            new Address
            (
                "PL",
                "01-001",
                "Warsaw",
                "Zielona 7"
            )
        );
        var propTwo = new Property
        (
            new MonetaryAmount(100000),
            new Address
            (
                "PL",
                "01-001",
                "Warsaw",
                "Zielona 8"
            )
        );
            
        propOne.Equals(propTwo).Should().BeFalse();
    }
}