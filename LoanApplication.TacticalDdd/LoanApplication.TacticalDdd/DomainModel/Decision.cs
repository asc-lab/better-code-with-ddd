using System;
using System.Collections.Generic;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Decision : ValueObject<Decision>
    {
        public DateTime DecisionDate { get;  }
        public Guid DecisionBy { get;  }

        public Decision(DateTime decisionDate, Operator decisionBy)
            : this(decisionDate,decisionBy.Id)
        {
        }
        
        [JsonConstructor]
        public Decision(DateTime decisionDate, Guid decisionBy)
        {
            DecisionDate = decisionDate;
            DecisionBy = decisionBy;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return DecisionDate;
            yield return DecisionBy;
        }
    }
}