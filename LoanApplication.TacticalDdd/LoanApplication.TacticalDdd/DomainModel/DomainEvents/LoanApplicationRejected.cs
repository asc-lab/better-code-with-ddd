using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel.DomainEvents;

public class LoanApplicationRejected : DomainEvent
{
    public Guid LoanApplicationId { get; }

    public LoanApplicationRejected(LoanApplication loanApplication)
        : this(loanApplication.Id.Value)
    {
    }
        
    [JsonConstructor]
    protected LoanApplicationRejected(Guid id)
    {
        LoanApplicationId = id;
    }
}