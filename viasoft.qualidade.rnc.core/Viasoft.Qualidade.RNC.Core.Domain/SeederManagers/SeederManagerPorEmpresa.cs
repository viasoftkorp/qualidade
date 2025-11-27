using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;

[Table("SeederManagerPorEmpresa")]
public class SeederManagerPorEmpresa : Entity, IMustHaveTenant, IMustHaveEnvironment, IMustHaveCompany
{
    public Guid TenantId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid CompanyId { get; set; }
    public bool InserirProdutosEmpresasSeederFinalizado { get; set; }
    
    public SeederManagerPorEmpresa() { }
}