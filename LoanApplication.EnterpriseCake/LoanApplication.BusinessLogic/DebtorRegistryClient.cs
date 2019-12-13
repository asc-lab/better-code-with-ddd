using System.Collections.Generic;
using System.Threading.Tasks;
using RestEase;

namespace LoanApplication.BusinessLogic
{
    public interface IDebtorRegistry
    {
        [Get("{pesel}")]
        Task<DebtorInfo> Get([Path] string pesel);
    }
    
    public class DebtorInfo
    {
        public string Pesel { get; set; }
        public List<Debt> Debts { get; set; }
    }

    public class Debt
    {
        public decimal Amount { get; set; }
    }
    
    public class DebtorRegistryClient
    {
        public async Task<DebtorInfo> GetDebtorInfo(string pesel)
        {
            return await RestClient.For<IDebtorRegistry>("http://localhost:5005/DebtorInfo").Get(pesel);
        }
    }
}