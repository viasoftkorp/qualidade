using System;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;

public class ConfiguracaoGeral : FullAuditedEntity, IMustHaveTenant, IMustHaveEnvironment
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public bool ConsiderarApenasSaldoApontado { get; set; }

    public void Update(ConfiguracaoGeralModel configuracaoGeral)
    {
        ConsiderarApenasSaldoApontado = configuracaoGeral.ConsiderarApenasSaldoApontado;
    }
}