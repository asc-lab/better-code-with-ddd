using LoanApplication.TacticalDdd.DomainModel.Ddd;
using System.Text.Json.Serialization;

namespace LoanApplication.TacticalDdd.DomainModel.DomainEvents;

public class LoanApplicationAccepted : DomainEvent
{
    public Guid LoanApplicationId { get; }

    public LoanApplicationAccepted(LoanApplication loanApplication)
        : this(loanApplication.Id.Value)
    {
    }
        
    [JsonConstructor]
    protected LoanApplicationAccepted(Guid loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
    }
}