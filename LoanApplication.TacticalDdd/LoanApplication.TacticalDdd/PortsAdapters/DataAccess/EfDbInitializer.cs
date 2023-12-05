using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess;

public class EfDbInitializer(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dbCtx = scope.ServiceProvider.GetService<LoanDbContext>();

        if (!dbCtx.Operators.Any(o=>o.Login=="admin"))
        {
            dbCtx.Operators.Add(new Operator
            (
                new Login("admin"),
                new Password("admin"),
                new Name("admin","admin"),
                new MonetaryAmount(1_000_000M)
            ));

        }

        await dbCtx.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}