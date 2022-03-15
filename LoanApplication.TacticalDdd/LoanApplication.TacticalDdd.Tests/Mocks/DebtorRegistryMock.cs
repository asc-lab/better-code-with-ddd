using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Mocks;

public class DebtorRegistryMock : IDebtorRegistry
{
    public const string DebtorNationalIdentifier = "11111111116";
    public bool IsRegisteredDebtor(Customer customer)
    {
        return customer.NationalIdentifier == new NationalIdentifier(DebtorNationalIdentifier);
    }
}