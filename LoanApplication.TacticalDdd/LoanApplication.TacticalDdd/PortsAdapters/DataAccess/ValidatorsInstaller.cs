using FluentValidation;
using LoanApplication.TacticalDdd.Application.Api;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public static class ValidatorsInstaller
{
    public static void AddFluentValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoanApplicationSubmissionDto>, LoanApplicationSubmissionDtoValidator>();
    }
}