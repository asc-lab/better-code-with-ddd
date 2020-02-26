using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.PortsAdapters.DataAccess;
using LoanApplication.TacticalDdd.PortsAdapters.ExternalServices;
using LoanApplication.TacticalDdd.PortsAdapters.MessageQueue;
using LoanApplication.TacticalDdd.PortsAdapters.Security;
using LoanApplication.TacticalDdd.ReadModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoanApplication.TacticalDdd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            
            services.AddEfDbAdapters(Configuration.GetConnectionString("LoanDb"));
            
            services.AddRabbitMqClient("host=localhost");
            
            services.AddExternalServicesClients();

            services.AddApplicationServices();

            services.AddReadModelServices(Configuration.GetConnectionString("LoanDb"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}