using Microsoft.Extensions.DependencyInjection;

namespace LoanApplication.TacticalDdd.Application
{
    public static class ApplicationServicesInstaller
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<LoanApplicationSubmissionService>();
            services.AddScoped<LoanApplicationEvaluationService>();
            services.AddScoped<LoanApplicationDecisionService>();
        }
    }
}