using System;
using System.Collections.Generic;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Newtonsoft.Json;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class Registration : ValueObject<Registration>
    {
        public DateTime RegistrationDate { get; }
        
        public OperatorId RegisteredBy { get;  }

        public Registration(DateTime registrationDate, Operator registeredBy)
            : this(registrationDate, registeredBy.Id)
        {
        }

        [JsonConstructor]
        public Registration(DateTime registrationDate, OperatorId registeredBy)
        {
            RegistrationDate = registrationDate;
            RegisteredBy = registeredBy;
        }
        
        //To satisfy EF Core
        protected Registration()
        {
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            yield return RegistrationDate;
            yield return RegisteredBy;
        }
    }
}