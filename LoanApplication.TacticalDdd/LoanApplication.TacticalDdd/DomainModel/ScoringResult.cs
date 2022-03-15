using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel;

public class ScoringResult : ValueObject<ScoringResult>
{
    public ApplicationScore? Score { get; }
    public string Explanation { get; }

    [JsonConstructor]
    private ScoringResult(ApplicationScore? score, string explanation)
    {
        Score = score;
        Explanation = explanation;
    }
        
    //To satisfy EF Core
    protected ScoringResult()
    {
    }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Score;
        yield return Explanation;
    }

    public static ScoringResult Green()
    {
        return new ScoringResult(ApplicationScore.Green, null);
    }
        
    public static ScoringResult Red(string[] messages)
    {
        return new ScoringResult(ApplicationScore.Red, string.Join(Environment.NewLine,messages));
    }

    public bool IsRed()
    {
        return Score == ApplicationScore.Red;
    }
}