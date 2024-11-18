using LoanApplication.TacticalDdd.DomainModel;
using Marten;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class DbInitializer : IHostedService
{
    private readonly IServiceProvider serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        using var session = scope.ServiceProvider.GetService<IDocumentStore>().LightweightSession();

        if (!session.Query<Operator>().Any(o => o.Login == "admin"))
            session.Insert(new Operator
            (
                "admin",
                "admin",
                "admin",
                "admin",
                new MonetaryAmount(1_000_000M)
            ));

        await session.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}