using System;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel.DomainEvents
{
    public class LoanApplicationAccepted : DomainEvent
    {
        public Guid LoanApplicationId { get; }

        public LoanApplicationAccepted(LoanApplication loanApplication)
            : this(loanApplication.Id.Value)
        {
        }
        
        [JsonConstructor]
        protected LoanApplicationAccepted(Guid id)
        {
            LoanApplicationId = id;
        }
    }
}