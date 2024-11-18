using LoanApplication.TacticalDdd.DomainModel;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.DomainTests;

public class MonetaryAmountTests
{
    [Fact]
    public void The_same_amounts_are_equal()
    {
        var one = new MonetaryAmount(10M);
        var two = new MonetaryAmount(10M);

        Assert.Equal(one, two);
        Assert.True(one == two);
    }

    [Fact]
    public void Not_the_same_amounts_are_not_equal()
    {
        var one = new MonetaryAmount(10M);
        var two = new MonetaryAmount(11M);

        Assert.NotEqual(one, two);
        Assert.True(one != two);
    }

    [Fact]
    public void Two_amounts_can_be_compared()
    {
        var one = new MonetaryAmount(10M);
        var two = new MonetaryAmount(11M);

        Assert.True(one < two);
        Assert.False(one > two);
    }

    [Fact]
    public void Can_add()
    {
        var one = new MonetaryAmount(10M);
        var two = new MonetaryAmount(11M);

        var sum = one + two;

        Assert.Equal(sum, new MonetaryAmount(21M));
    }

    [Fact]
    public void Can_subtract()
    {
        var one = new MonetaryAmount(10M);
        var two = new MonetaryAmount(5M);

        var diff = one - two;

        Assert.Equal(diff, new MonetaryAmount(5M));
    }

    [Fact]
    public void Can_multiply_by_percent()
    {
        var one = new MonetaryAmount(10M);
        var tenPercent = 10.Percent();

        var percentOfOne = one * tenPercent;

        Assert.Equal(percentOfOne, new MonetaryAmount(1M));
    }
}