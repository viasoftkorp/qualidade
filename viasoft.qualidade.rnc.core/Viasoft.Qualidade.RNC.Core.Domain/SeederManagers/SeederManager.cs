using System;
using System.ComponentModel.DataAnnotations.Schema;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;

namespace Viasoft.Qualidade.RNC.Core.Domain.SeederManagers
{
    [Table("SeederManager")]
    public class SeederManager : Entity, IMustHaveTenant, IMustHaveEnvironment
    {
        public Guid TenantId { get; set; }
        public Guid EnvironmentId { get; set; }

        public bool PreencherIdCategoriaProdutosSeederFinalizado { get; set; }
        public bool PreencherCodigoRecursosSeederFinalizado { get; set; }
        public bool PreencherCentroCustosSeederFinalizado { get; set; }
        public bool ConverterNumeroOdfParaNumeroOdfAsIntSeederFinalizado { get; set; }
        public bool PreencherDataCriacaoNaoConformidadeSeederFinalizado { get; set; }
        public bool ConverterIdPedidoParaNumeroPedidoSeederFinalizado { get; set; }
        public bool CorrigirNaoConformidadesFechadasSemConclusaoSeederFinalizado { get; set; }
        public bool PreencherLocaisSeederFinalizado { get; set; }
        public bool PreencherIdsCausasCentrosCustosNaoConformidadesSeederFinalizado { get; set; }
        public bool CorrigirNumeroNotaFiscalNaoConformidadesSeederFinalizado { get; set; }
        public bool CorrigirUsuariosSolucoesSeederFinalizado { get; set; }
    }
}
