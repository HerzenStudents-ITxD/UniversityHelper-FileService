using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using UniversityHelper.FileService.Broker.Consumers;
using UniversityHelper.FileService.Data.Provider.MsSql.Ef;
using UniversityHelper.FileService.Models.Dto.Configurations;
using UniversityHelper.Core.BrokerSupport.Configurations;
using UniversityHelper.Core.BrokerSupport.Extensions;
using UniversityHelper.Core.BrokerSupport.Helpers;
using UniversityHelper.Core.BrokerSupport.Middlewares.Token;
using UniversityHelper.Core.Configurations;
using UniversityHelper.Core.EFSupport.Extensions;
using UniversityHelper.Core.EFSupport.Helpers;
using UniversityHelper.Core.Extensions;
using UniversityHelper.Core.Middlewares.ApiInformation;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace UniversityHelper.FileService
{
  public class Startup : BaseApiInfo
  {
    public const string CorsPolicyName = "LtDoCorsPolicy";

    private readonly BaseServiceInfoConfig _serviceInfoConfig;
    private readonly RabbitMqConfig _rabbitMqConfig;

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      _serviceInfoConfig = Configuration
        .GetSection(BaseServiceInfoConfig.SectionName)
        .Get<BaseServiceInfoConfig>();

      _rabbitMqConfig = Configuration
        .GetSection(BaseRabbitMqConfig.SectionName)
        .Get<RabbitMqConfig>();

      Version = "2.0.2.0";
      Description = "FileService is an API intended to work with files and images.";
      StartTime = DateTime.UtcNow;
      ApiName = $"UniversityHelper - {_serviceInfoConfig.Name}";
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(
          CorsPolicyName,
          builder =>
            {
              builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
      });

      services.AddHttpContextAccessor();

      services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
      services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));
      services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));

      string connStr = ConnectionStringHandler.Get(Configuration);

      services.AddDbContext<FileServiceDbContext>(options =>
      {
        options.UseSqlServer(connStr);
      });

      services.AddBusinessObjects();

      services
        .AddControllers()
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddNewtonsoftJson();

      services.ConfigureMassTransit(_rabbitMqConfig);

      services.AddHealthChecks()
        .AddSqlServer(connStr)
        .AddRabbitMqCheck();

      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc($"{Version}", new OpenApiInfo
        {
          Version = Version,
          Title = _serviceInfoConfig.Name,
          Description = Description
        });

        options.EnableAnnotations();
      });
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
      app.UpdateDatabase<FileServiceDbContext>();

      app.UseForwardedHeaders();

      app.UseExceptionsHandler(loggerFactory);

      app.UseApiInformation();

      app.UseRouting();

      app.UseMiddleware<TokenMiddleware>();

      app.UseCors(CorsPolicyName);

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers().RequireCors(CorsPolicyName);

        endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
        {
          ResultStatusCodes = new Dictionary<HealthStatus, int>
            {
              { HealthStatus.Unhealthy, 200 },
              { HealthStatus.Healthy, 200 },
              { HealthStatus.Degraded, 200 },
            },
          Predicate = check => check.Name != "masstransit-bus",
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
      });

      app.UseSwagger()
        .UseSwaggerUI(options =>
        {
          options.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"{Version}");
        });
    }
  }
}
