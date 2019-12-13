using LoanApplication.Infrastructure.Common;

namespace LoanApplication.BusinessLogic
{
    public class Property : Entity
    {
        public decimal Value { get; set; }
        public virtual Address Address { get; set; }
    }
}