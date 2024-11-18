using LoanApplication.TacticalDdd.DomainModel;
using Xunit;

namespace LoanApplication.TacticalDdd.Tests.Asserts;

public class LoanApplicationAssert
{
    private readonly DomainModel.LoanApplication loanApplication;

    public LoanApplicationAssert(DomainModel.LoanApplication loanApplication)
    {
        this.loanApplication = loanApplication;
    }

    public static LoanApplicationAssert That(DomainModel.LoanApplication loanApplication)
    {
        return new LoanApplicationAssert(loanApplication);
    }

    public LoanApplicationAssert IsInStatus(LoanApplicationStatus expectedStatus)
    {
        Assert.Equal(expectedStatus, loanApplication.Status);
        return this;
    }

    public LoanApplicationAssert ScoreIsNull()
    {
        Assert.Null(loanApplication.Score);
        return this;
    }

    public LoanApplicationAssert ScoreIs(ApplicationScore expectedScore)
    {
        Assert.Equal(expectedScore, loanApplication.Score?.Score);
        return this;
    }
}