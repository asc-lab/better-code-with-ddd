using LoanApplication.Infrastructure.Common;

namespace LoanApplication.BusinessLogic
{
    public class Operator : Entity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal CompetenceLevel { get; set; }
    }
}