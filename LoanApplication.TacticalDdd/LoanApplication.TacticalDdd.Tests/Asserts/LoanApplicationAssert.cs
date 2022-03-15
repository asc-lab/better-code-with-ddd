using FluentAssertions;
using FluentAssertions.Primitives;
using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Asserts;

public static class LoanApplicationAssertExtension
{
    public static LoanApplicationAssert Should(this DomainModel.LoanApplication loanApplication) 
        => new LoanApplicationAssert(loanApplication);
}

public class LoanApplicationAssert : ReferenceTypeAssertions<DomainModel.LoanApplication,LoanApplicationAssert>
{
    public LoanApplicationAssert(DomainModel.LoanApplication loanApplication)
        : base(loanApplication)
    {
            
    }
        
    public AndConstraint<LoanApplicationAssert> BeInStatus(LoanApplicationStatus expectedStatus)
    {
        Subject.Status.Should().Be(expectedStatus);
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
        Subject.Score.Should().BeNull();
        return new AndConstraint<LoanApplicationAssert>(this);
    }
        
    public AndConstraint<LoanApplicationAssert> ScoreIs(ApplicationScore expectedScore)
    {
        Subject.Score?.Score.Should().Be(expectedScore);
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

    protected override string Identifier => "LoanApplicationAssert";
}