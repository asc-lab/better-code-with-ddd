using FluentValidation;

namespace LoanApplication.TacticalDdd.Application.Api;

public static class ValidatorsInstaller
{
    public static void AddFluentValidators(this IServiceCollection services) => 
        services.AddScoped<IValidator<LoanApplicationSubmissionDto>, LoanApplicationSubmissionDtoValidator>();
    
}