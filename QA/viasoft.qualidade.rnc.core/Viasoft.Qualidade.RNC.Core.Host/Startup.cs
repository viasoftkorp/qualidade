using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.API.Administration.Extensions;
using Viasoft.Core.API.Authentication.Extensions;
using Viasoft.Core.API.Authorization.Extensions;
using Viasoft.Core.API.EmailTemplate.Extensions;
using Viasoft.Core.API.LicensingManagement.Extensions;
using Viasoft.Core.API.Reporting.Extensions;
using Viasoft.Core.API.TenantManagement.Extensions;
using Viasoft.Core.API.UserProfile.Extensions;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.AspNetCore.ApiVersioning;
using Viasoft.Core.AspNetCore.Extensions;
using Viasoft.Core.AspNetCore.Provisioning;
using Viasoft.Core.AspNetCore.UnitOfWork;
using Viasoft.Core.Authorization.AspNetCore.Extensions;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.EntityFrameworkCore.PostgreSQL.Extensions;
using Viasoft.Core.Identity.AspNetCore.Extensions;
using Viasoft.Core.IoC.Extensions;
using Viasoft.Core.Legacy.Parametros.Extensions;
using Viasoft.Core.MultiTenancy.AspNetCore.Extensions;
using Viasoft.Core.Service;
using Viasoft.Core.ServiceBus.AspNetCore.Extensions;
using Viasoft.Core.ServiceBus.PostgreSQL.Extensions;
using Viasoft.Core.ServiceDiscovery.Extensions;
using Viasoft.Core.Storage.Extensions;
using Viasoft.Data.Seeder.Extensions;
using Viasoft.PushNotifications.AspNetCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Host.FrontendUrls;
using Viasoft.Qualidade.RNC.Core.Host.Seeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNaoConformidadesFechadasSemConclusaoSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNumeroNotaFiscalNaoConformidadesSeeder;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirUsuariosSolucoesSeeder;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCentrosCustosSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCodigoRecursosSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherDataCriacaoNaoConformidadesSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdCategoriaProdutosSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdsCausasCentrosCustosNaoConformidadesSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherLocaisSeeders;
using Viasoft.Qualidade.RNC.Core.Infrastructure.EntityFrameworkCore;

namespace Viasoft.Qualidade.RNC.Core.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public static IServiceConfiguration ServiceConfiguration => new ServiceConfiguration
        {
            ServiceName = "Viasoft.Qualidade.RNC.Core",
            Domain = "Qualidade",
            App = "RNC",
            Area = "Core",
        };

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AspNetCoreDefaultConfiguration(options =>
                {
                    options.UseNewSerializer = true;
                }, ServiceConfiguration, _configuration);

            services
                .AddPersistence(_configuration)
                .AddEfCore<ViasoftQualidadeRNCCoreDbContext>()
                .AddEfCorePostgreSql()
                .AddServiceBus(options =>
                {
                    options.CompressionOptions.Enabled = true;
                    options.TimeoutOptions.Enabled = true;
                }, ServiceConfiguration, _configuration)
                .AddServiceBusPostgreSqlProvider()
                .AddServiceMesh()
                .AddApiClient(_configuration)
                .AddMultiTenancy()
                .AddDomainDrivenDesign()
                .RegisterDependenciesByConvention()
                .AddNotification()
                .AddUserIdentity()
                // TODO alterar para AcceptsOpenIdScope = false, quando for ajeitado a lib do rnc
                .AddAuthorizations(_configuration, options => options.AcceptsOpenIdScope = true)
                .AddAuthorizationApi()
                .AddAdministrationApi()
                .AddAuthenticationApi()
                .AddEmailTemplateApi()
                .AddLegacyParametros()
                .AddLicensingManagementApi()
                .AddReportingApi()
                .AddSingleton<IFrontendUrl, FrontendUrl>()
                .AddVersioning(_configuration)
                .AddSeeders(new List<Type>
                {
                    typeof(AdicionaSeederManagerSeeder),
                    typeof(AdicionaConfiguracaoGeralSeeder),
                    typeof(PreencherIdCategoriaProdutosSeeder),
                    typeof(PreencherCodigoRecursosSeeder),
                    typeof(PreencherCentroCustosSeeder),
                    typeof(PreencherDataCriacaoNaoConformidadeSeeder),
                    typeof(CorrigirNaoConformidadesFechadasSemConclusaoSeeder),
                    typeof(PreencherLocaisSeeder),
                    typeof(PreencherIdsCausasCentrosCustosNaoConformidadesSeeder),
                    typeof(CorrigirNumeroNotaFiscalNaoConformidadesSeeder),
                    typeof(CorrigirUsuariosSolucoesSeeder)
                })
                .AddTenantManagementApi()
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
