using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanApplication.BusinessLogic;
using LoanApplication.DataAccess;
using LoanApplication.Infrastructure.DataAccess;
using LoanApplication.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoanApplication.WebApi
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
            
            services.AddScoped<LoanApplicationService>();
            services.AddScoped<ValidationService>();
            services.AddScoped<ScoringService>();
            services.AddSingleton<DebtorRegistryClient>();

            services.AddDbContext<LoanApplicationDbContext>(opts =>
            {
                opts.UseInMemoryDatabase("Loans");
                opts.UseLazyLoadingProxies();
            });
            services.AddHostedService<DbInitializer>();
            
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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