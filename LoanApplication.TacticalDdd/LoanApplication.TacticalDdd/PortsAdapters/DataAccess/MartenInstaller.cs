using LoanApplication.TacticalDdd.DomainModel;
using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using IUnitOfWork = LoanApplication.TacticalDdd.DomainModel.Ddd.IUnitOfWork;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess
{
    public static class MartenInstaller
    {
        public static void AddMartenDbAdapters(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(CreateDocumentStore(cnnString));
            services.AddScoped(svc => svc.GetService<IDocumentStore>().DirtyTrackedSession());
            services.AddScoped<IUnitOfWork, MartenUnitOfWork>();
            services.AddScoped<ILoanApplicationRepository, MartenLoanApplicationRepository>();
            services.AddScoped<IOperatorRepository, MartenOperatorRepository>();
            services.AddHostedService<DbInitializer>();
        }

        private static IDocumentStore CreateDocumentStore(string cn)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(cn);
                _.DatabaseSchemaName = "ddd_loan";
                _.Serializer(CustomizeJsonSerializer());

                _.Schema.For<DomainModel.LoanApplication>()
                    .Duplicate(t => t.Number,pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
            });
        }

        private static JsonNetSerializer CustomizeJsonSerializer()
        {
            var serializer = new JsonNetSerializer();

            serializer.Customize(_ =>
            {
                _.ContractResolver = new ProtectedSettersContractResolver();
                _.Converters.Add(new StringEnumConverter());
            });

            return serializer;
        }
    }
}