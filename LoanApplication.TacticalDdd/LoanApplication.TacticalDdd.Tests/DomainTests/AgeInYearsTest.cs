using FluentAssertions;
using LoanApplication.TacticalDdd.DomainModel;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.DomainTests;

public class AgeInYearsTest
{
    [Fact]
    public void AgeInYears_PersonBorn1974_AfterBirthdateIn2019_45()
    {
        var age = AgeInYears.Between(new DateOnly(1974, 6, 26), new DateOnly(2019, 11, 28));
           
        age.Should().Be(45.Years());
    }
        
    [Fact]
    public void AgeInYears_PersonBorn1974_BeforeBirthdateIn2019_45()
    {
        var age = AgeInYears.Between(new DateOnly(1974, 6, 26), new DateOnly(2019, 5, 28));
            
        age.Should().Be(45.Years());
    }
}