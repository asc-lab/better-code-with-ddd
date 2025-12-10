using Carter;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.Infrastructure.DataAccess;
using LoanApplication.TacticalDdd.Infrastructure.ExternalServices;
using LoanApplication.TacticalDdd.Infrastructure.MessageQueue;
using LoanApplication.TacticalDdd.Security;
using LoanApplication.TacticalDdd.ReadModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi;

// using Microsoft.OpenApi.Models; // removed to avoid dependency on Microsoft.OpenApi types

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", opts =>
    {
        opts.TimeProvider = TimeProvider.System;
    });
builder.Services.AddAuthorization();
builder.Services.AddFluentValidators();
builder.Services.AddEfDbAdapters(builder.Configuration);
builder.Services.AddRabbitMqClient("host=localhost");
builder.Services.AddExternalServicesClients();
builder.Services.AddApplicationServices();
builder.Services.AddReadModelServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Name = "Authorization"
    });
    
    /*opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            Array.Empty<string>()
        }
    });*/
    opts.AddSecurityRequirement(doc =>
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("basic", doc),
                []
            }
        };
    });
});
builder.Services.AddCarter();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapCarter();

app.Run();