namespace LoanApplication.TacticalDdd.ReadModel;

public static class ReadModelInstaller
{
    public static void AddReadModelServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(_ => new LoanApplicationFinder(connectionString));
    }
}