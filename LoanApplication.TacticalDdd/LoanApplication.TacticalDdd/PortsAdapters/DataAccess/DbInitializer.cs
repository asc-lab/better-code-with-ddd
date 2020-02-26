using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.TacticalDdd.DomainModel;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess
{
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

            if (!session.Query<Operator>().Any(o=>o.Login=="admin"))
            {
                session.Insert(new Operator
                (
                    new Login("admin"),
                    new Password("admin"),
                    new Name("admin","admin"),
                    new MonetaryAmount(1_000_000M)
                ));

            }

            await session.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}