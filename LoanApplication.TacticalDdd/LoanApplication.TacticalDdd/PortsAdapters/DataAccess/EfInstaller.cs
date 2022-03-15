using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public static class EfInstaller
{
    public static void AddEfDbAdapters(this IServiceCollection services, string cnnString)
    {
        services.AddDbContext<LoanDbContext>(opts => { opts.UseNpgsql(cnnString); });
            
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<ILoanApplicationRepository, EfLoanApplicationRepository>();
        services.AddScoped<IOperatorRepository, EfOperatorRepository>();
        services.AddHostedService<EfDbInitializer>();
    }
}