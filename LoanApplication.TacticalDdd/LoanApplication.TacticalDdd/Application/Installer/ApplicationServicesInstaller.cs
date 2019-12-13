using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApplication.TacticalDdd.Application.Installer
{
    public static class ApplicationServicesInstaller
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationServicesInstaller).Assembly);        
        }
    }
}