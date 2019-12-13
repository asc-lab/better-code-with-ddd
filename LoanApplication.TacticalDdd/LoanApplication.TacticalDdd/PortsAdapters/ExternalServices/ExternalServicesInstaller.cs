using LoanApplication.TacticalDdd.DomainModel;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApplication.TacticalDdd.PortsAdapters.ExternalServices
{
    public static class ExternalServicesInstaller
    {
        public static void AddExternalServicesClients(this IServiceCollection services)
        {
            services.AddSingleton<IDebtorRegistry, DebtorRegistry>();
        }
    }
}