using FluentAssertions;
using FluentAssertions.Primitives;
using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Asserts;

public static class LoanApplicationAssertExtension
{
    public static LoanApplicationAssert Should(this DomainModel.LoanApplication loanApplication) 
        => new LoanApplicationAssert(loanApplication);
}

public class LoanApplicationAssert(DomainModel.LoanApplication loanApplication)
{
    public AndConstraint<LoanApplicationAssert> BeInStatus(LoanApplicationStatus expectedStatus)
    {
        loanApplication.Status.Should().Be(expectedStatus);
        return new AndConstraint<LoanApplicationAssert>(this);
    }
        
    public AndConstraint<LoanApplicationAssert> BeAccepted()
    {
        return BeInStatus(LoanApplicationStatus.Accepted);
    }
        
    public AndConstraint<LoanApplicationAssert> BeRejected()
    {
        return BeInStatus(LoanApplicationStatus.Rejected);
    }
        
    public AndConstraint<LoanApplicationAssert> BeNew()
    {
        return BeInStatus(LoanApplicationStatus.New);
    }
        
    public AndConstraint<LoanApplicationAssert> ScoreIsNull()
    {
        loanApplication.Score.Should().BeNull();
        return new AndConstraint<LoanApplicationAssert>(this);
    }
        
    public AndConstraint<LoanApplicationAssert> ScoreIs(ApplicationScore expectedScore)
    {
        loanApplication.Score?.Score.Should().Be(expectedScore);
        return new AndConstraint<LoanApplicationAssert>(this);
    }
        
    public AndConstraint<LoanApplicationAssert> HaveRedScore()
    {
        return ScoreIs(ApplicationScore.Red);
    }
        
    public AndConstraint<LoanApplicationAssert> HaveGreenScore()
    {
        return ScoreIs(ApplicationScore.Green);
    }

}