using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;

public class AgregacaoNaoConformidadeOutput
{
    public NaoConformidadeOutput NaoConformidade { get; init; }
    public ConclusaoNaoConformidadeOutput ConclusaoNaoConformidade { get; init; }
    public ReclamacaoNaoConformidadeOutput ReclamacaoNaoConformidade { get; init; }
    public OrdemRetrabalhoNaoConformidadeOutput OrdemRetrabalhoNaoConformidade { get; set; }

    public List<AcaoPreventivaNaoConformidadeOutput> AcaoPreventivaNaoConformidades { get; init; }
    public List<CausaNaoConformidadeOutput> CausaNaoConformidades { get; init; }
    public List<DefeitoNaoConformidadeOutput> DefeitoNaoConformidades { get; init; }
    public List<SolucaoNaoConformidadeOutput> SolucaoNaoConformidades { get; init; }
    public List<ProdutoNaoConformidadeOutput> ProdutoNaoConformidades { get; init; }
    public List<ServicoNaoConformidadeOutput> ServicoNaoConformidades { get; init; }
    public List<CentroCustoCausaNaoConformidadeOutput> CentroCustoCausaNaoConformidades { get; init; }
    public List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput> ImplementacaoEvitarReincidencia { get; init; }


    public AgregacaoNaoConformidadeOutput()
    {
        AcaoPreventivaNaoConformidades = new List<AcaoPreventivaNaoConformidadeOutput>();
        CausaNaoConformidades = new List<CausaNaoConformidadeOutput>();
        DefeitoNaoConformidades = new List<DefeitoNaoConformidadeOutput>();
        SolucaoNaoConformidades = new List<SolucaoNaoConformidadeOutput>();
        ProdutoNaoConformidades = new List<ProdutoNaoConformidadeOutput>();
        ServicoNaoConformidades = new List<ServicoNaoConformidadeOutput>();
        CentroCustoCausaNaoConformidades = new List<CentroCustoCausaNaoConformidadeOutput>();
        ImplementacaoEvitarReincidencia = new List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput>();
    }

    public AgregacaoNaoConformidadeOutput(NaoConformidadeOutput naoConformidade,
        List<AcaoPreventivaNaoConformidadeOutput> acoesPreventivasNaoConformidades,
        List<CausaNaoConformidadeOutput> causasNaoConformidades,
        List<DefeitoNaoConformidadeOutput> defeitosNaoConformidades,
        List<SolucaoNaoConformidadeOutput> solucoesNaoConformidades,
        List<ProdutoNaoConformidadeOutput> produtosNaoConformidades,
        List<ServicoNaoConformidadeOutput> servicosNaoConformidades,
        List<CentroCustoCausaNaoConformidadeOutput> centroCustoCausaNaoConformidades,
        ConclusaoNaoConformidadeOutput conclusaoNaoConformidade,
        ReclamacaoNaoConformidadeOutput reclamacaoNaoConformidade,
        List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput> implementacaoEvitarReincidenciaNaoConformidade) : this()
    {
        NaoConformidade = naoConformidade;
        AcaoPreventivaNaoConformidades = acoesPreventivasNaoConformidades;
        CausaNaoConformidades = causasNaoConformidades;
        DefeitoNaoConformidades = defeitosNaoConformidades;
        SolucaoNaoConformidades = solucoesNaoConformidades;
        ProdutoNaoConformidades = produtosNaoConformidades;
        ServicoNaoConformidades = servicosNaoConformidades;
        CentroCustoCausaNaoConformidades = centroCustoCausaNaoConformidades;
        ConclusaoNaoConformidade = conclusaoNaoConformidade;
        ReclamacaoNaoConformidade = reclamacaoNaoConformidade;
        ImplementacaoEvitarReincidencia = implementacaoEvitarReincidenciaNaoConformidade;
    }
}
