using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel;

public class ScoringResult : ValueObject<ScoringResult>
{
    [JsonConstructor]
    private ScoringResult(ApplicationScore? score, string explanation)
    {
        Score = score;
        Explanation = explanation;
    }

    public ApplicationScore? Score { get; }
    public string Explanation { get; }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Score;
        yield return Explanation;
    }

    public static ScoringResult Green() => new ScoringResult(ApplicationScore.Green, null);
    
    public static ScoringResult Red(string[] messages) => new ScoringResult(ApplicationScore.Red, string.Join(Environment.NewLine, messages));

    public bool IsRed() => Score == ApplicationScore.Red;
}