using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.ExternalServices;

public class DebtorRegistry : IDebtorRegistry
{
    public bool IsRegisteredDebtor(Customer customer)
    {
        var client = new DebtorRegistryClient();
        var debtorInfo = client.GetDebtorInfo(customer.NationalIdentifier.Value).Result;

        return debtorInfo.Debts.Any();
    }
}