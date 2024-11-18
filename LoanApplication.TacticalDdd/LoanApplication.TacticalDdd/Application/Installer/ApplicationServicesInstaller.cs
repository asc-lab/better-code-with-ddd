using MediatR;

namespace LoanApplication.TacticalDdd.Application.Installer;

public static class ApplicationServicesInstaller
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
    }
}