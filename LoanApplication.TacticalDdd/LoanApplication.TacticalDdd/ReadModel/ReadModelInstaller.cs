namespace LoanApplication.TacticalDdd.ReadModel;

public static class ReadModelInstaller
{
    public static void AddReadModelServices(this IServiceCollection services, ConfigurationManager cfgManger)
    {
        services.AddSingleton(_ => new LoanApplicationFinder(cfgManger.GetConnectionString("LoanDb")));
    }
}