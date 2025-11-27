using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class DefeitoDataSource
{
    public List<DefeitoNaoConformidadeViewOutput> DefeitosNaoConformidade { get; set; }
}