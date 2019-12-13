using System;
using System.Collections.Generic;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Decision : ValueObject<Decision>
    {
        public DateTime DecisionDate { get;  }
        public OperatorId DecisionBy { get;  }

        public Decision(DateTime decisionDate, Operator decisionBy)
            : this(decisionDate,decisionBy.Id)
        {
        }
        
        [JsonConstructor]
        public Decision(DateTime decisionDate, OperatorId decisionBy)
        {
            DecisionDate = decisionDate;
            DecisionBy = decisionBy;
        }

        // To Satisfy EF Core
        protected Decision()
        {
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return DecisionDate;
            yield return DecisionBy;
        }
    }
}