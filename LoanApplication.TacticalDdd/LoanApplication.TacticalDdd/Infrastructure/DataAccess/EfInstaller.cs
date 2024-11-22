using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.TacticalDdd.Infrastructure.DataAccess;

public static class EfInstaller
{
    public static void AddEfDbAdapters(this IServiceCollection services, ConfigurationManager cfgManager)
    {
        services.AddDbContext<LoanDbContext>(opts =>
        {
            opts
                .UseNpgsql(cfgManager.GetConnectionString("LoanDb"));
        });
            
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<ILoanApplicationRepository, EfLoanApplicationRepository>();
        services.AddScoped<IOperatorRepository, EfOperatorRepository>();
        services.AddHostedService<EfDbInitializer>();
    }
}