using LoanApplication.Infrastructure.Common;

namespace LoanApplication.BusinessLogic
{
    public class Address : Entity
    {
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}