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
    public class EfDbInitializer : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public EfDbInitializer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

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
}