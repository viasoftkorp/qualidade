using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;

public class CausaDataSource
{
    public List<CausaNaoConformidadeViewOutput> CausasNaoConformidade { get; set; }
}