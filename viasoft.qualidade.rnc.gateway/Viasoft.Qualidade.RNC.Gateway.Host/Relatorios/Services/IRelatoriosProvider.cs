using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Services;

public interface IRelatoriosProvider
{
    Task<ExportarRelatorioNaoConformidadeOutput> ExportarRelatorio(Guid idNaoConformidade);
}