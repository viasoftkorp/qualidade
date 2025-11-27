using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.API.Administration.Extensions;
using Viasoft.Core.API.Authentication.Extensions;
using Viasoft.Core.API.Authorization.Extensions;
using Viasoft.Core.API.EmailTemplate.Extensions;
using Viasoft.Core.API.LicensingManagement.Extensions;
using Viasoft.Core.API.Reporting.Extensions;
using Viasoft.Core.API.TenantManagement.Extensions;
using Viasoft.Core.API.UserProfile.Extensions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.AspNetCore.ApiVersioning;
using Viasoft.Core.AspNetCore.Extensions;
using Viasoft.Core.AspNetCore.Provisioning;
using Viasoft.Core.AspNetCore.UnitOfWork;
using Viasoft.Core.Authentication.Proxy.Extensions;
using Viasoft.Core.Authorization.AspNetCore.Extensions;
using Viasoft.Core.Authorization.Proxy.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.EntityFrameworkCore.PostgreSQL.Extensions;
using Viasoft.Core.FileProvider.Extensions;
using Viasoft.Core.FileProvider.Proxy.Extensions;
using Viasoft.Core.Identity.AspNetCore.Extensions;
using Viasoft.Core.IoC.Extensions;
using Viasoft.Core.Legacy.Parametros.Extensions;
using Viasoft.Core.MultiTenancy.AspNetCore.Extensions;
using Viasoft.Core.Reporting.Extension;
using Viasoft.Core.Service;
using Viasoft.Core.ServiceBus.AspNetCore.Extensions;
using Viasoft.Core.ServiceBus.PostgreSQL.Extensions;
using Viasoft.Core.ServiceDiscovery.Extensions;
using Viasoft.Core.Storage.Extensions;
using Viasoft.Data.Seeder.Extensions;
using Viasoft.PushNotifications.AspNetCore.Extensions;
using Viasoft.Qualidade.RNC.Gateway.Host.Seeders.Relatorios;
using Viasoft.Qualidade.RNC.Gateway.Infrastructure.EntityFrameworkCore;

namespace Viasoft.Qualidade.RNC.Gateway.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public static IServiceConfiguration ServiceConfiguration => new ServiceConfiguration
        {
            ServiceName = "Viasoft.Qualidade.RNC.Gateway",
            Domain = "Qualidade",
            App = "RNC",
            Area = "Gateway",
        };

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AspNetCoreDefaultConfiguration(options => { options.UseNewSerializer = true; },
                ServiceConfiguration, _configuration);

            services
                .AddPersistence(_configuration)
                .AddEfCore<ViasoftQualidadeRNCGatewayDbContext>()
                .AddEfCorePostgreSql()
                .AddServiceBus(options => { options.CompressionOptions.Enabled = true; }, ServiceConfiguration,
                    _configuration)
                .AddServiceBusPostgreSqlProvider()
                .AddServiceMesh()
                .AddApiClient(_configuration, options => options.DefaultTimeoutPeriod = ApiServiceCallTimeout.Long)
                .AddMultiTenancy()
                .AddDomainDrivenDesign()
                .RegisterDependenciesByConvention()
                .AddNotification()
                .AddUserIdentity()
                .AddAuthorizations(_configuration, options => options.AcceptsOpenIdScope = true)
                .AddAuthorizationApi()
                .AddAdministrationApi()
                .AddAuthenticationApi()
                .AddEmailTemplateApi()
                .AddLegacyParametros()
                .AddLicensingManagementApi()
                .AddReportingApi()
                .AddReporting()
                .AddVersioning(_configuration)
                .AddSeeders(
                    new List<Type>
                    {
                        typeof(CriarRelatorioNaoConformidadeSeeder),
                    }
                )
                .AddTenantManagementApi()
                .AddAuthenticationProxy()
                .AddAuthorizationProxy()
                .AddFileProviderProxy()
                .AddFileProvider()
                .AddUserProfileApi();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.AspNetCoreDefaultAppConfiguration()
                .UseProvisioning()
                .UseUnitOfWork()
                .UseEndpoints();
        }
    }
}