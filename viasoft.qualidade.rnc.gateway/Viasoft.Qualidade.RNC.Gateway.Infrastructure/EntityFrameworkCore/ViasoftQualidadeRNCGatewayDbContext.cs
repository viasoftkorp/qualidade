using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Viasoft.Core.EntityFrameworkCore.Context;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Qualidade.RNC.Gateway.Infrastructure.EntityFrameworkCore
{
    public class ViasoftQualidadeRNCGatewayDbContext : BaseDbContext
    {
        public ViasoftQualidadeRNCGatewayDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider,
            ILoggerFactory loggerFactory,
            IBaseDbContextConfigurationService baseDbContextConfigurationService) : base(options, schemaNameProvider,
            loggerFactory, baseDbContextConfigurationService)
        {
        }
    }
}