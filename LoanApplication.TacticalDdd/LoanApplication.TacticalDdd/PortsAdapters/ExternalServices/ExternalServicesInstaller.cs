using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.ExternalServices;

public static class ExternalServicesInstaller
{
    public static void AddExternalServicesClients(this IServiceCollection services)
    {
        services.AddSingleton<IDebtorRegistry, DebtorRegistry>();
    }
}