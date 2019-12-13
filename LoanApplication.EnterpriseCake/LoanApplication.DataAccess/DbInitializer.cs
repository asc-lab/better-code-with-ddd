using System;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoanApplication.DataAccess
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

            var dbCtx = scope.ServiceProvider.GetService<LoanApplicationDbContext>();
            
            dbCtx.Operators.Add(new Operator
            {
                FirstName = "admin",
                LastName = "admin",
                Login = "admin",
                CompetenceLevel = 1_000_000M
            });

            await dbCtx.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}