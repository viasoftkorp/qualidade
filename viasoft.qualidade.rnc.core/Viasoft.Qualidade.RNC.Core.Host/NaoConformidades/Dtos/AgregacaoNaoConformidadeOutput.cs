using System.Collections.Generic;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

public class AgregacaoNaoConformidadeOutput
{
    public NaoConformidadeOutput NaoConformidade { get; set; }
    
    public ConclusaoNaoConformidadeOutput ConclusaoNaoConformidade { get; set; }
    
    public ReclamacaoNaoConformidadeOutput ReclamacaoNaoConformidade { get; set; }
    
    public OrdemRetrabalhoNaoConformidadeOutput OrdemRetrabalhoNaoConformidade { get; set; }
    
    public List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput> ImplementacaoEvitarReincidencia { get; set; }
    
    public List<AcaoPreventivaNaoConformidadeOutput> AcaoPreventivaNaoConformidades { get; set; }
    
    public List<CausaNaoConformidadeOutput> CausaNaoConformidades { get; set; }
    
    public List<DefeitoNaoConformidadeOutput> DefeitoNaoConformidades { get; set; }
   
    public List<SolucaoNaoConformidadeOutput> SolucaoNaoConformidades { get; set; }
   
    public List<ProdutoNaoConformidadeOutput> ProdutoNaoConformidades { get; set; }
    
    public List<ServicoNaoConformidadeOutput> ServicoNaoConformidades { get; set; }

    public List<CentroCustoCausaNaoConformidadeOutput> CentroCustoCausaNaoConformidades { get; set; }

    public AgregacaoNaoConformidadeOutput(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        NaoConformidade = new NaoConformidadeOutput(agregacaoNaoConformidade.NaoConformidade);

        if (agregacaoNaoConformidade.ConclusaoNaoConformidade != null)
        {
            ConclusaoNaoConformidade =
                new ConclusaoNaoConformidadeOutput(agregacaoNaoConformidade.ConclusaoNaoConformidade);
        }

        if (agregacaoNaoConformidade.ReclamacaoNaoConformidade != null)
        {
            ReclamacaoNaoConformidade =
                new ReclamacaoNaoConformidadeOutput(agregacaoNaoConformidade.ReclamacaoNaoConformidade);
        }
        
        if (agregacaoNaoConformidade.OrdemRetrabalhoNaoConformidade != null)
        {
            OrdemRetrabalhoNaoConformidade =
                new OrdemRetrabalhoNaoConformidadeOutput(agregacaoNaoConformidade.OrdemRetrabalhoNaoConformidade);
        } 

        ImplementacaoEvitarReincidencia =
            agregacaoNaoConformidade.ImplementacaoEvitarReincidencia.ConvertAll(e =>
                new ImplementacaoEvitarReincidenciaNaoConformidadeOutput(e));
        
        AcaoPreventivaNaoConformidades =
            agregacaoNaoConformidade.AcaoPreventivaNaoConformidades.ConvertAll(e =>
                new AcaoPreventivaNaoConformidadeOutput(e));
        
        CausaNaoConformidades =
            agregacaoNaoConformidade.CausaNaoConformidades.ConvertAll(e =>
                new CausaNaoConformidadeOutput(e));
        
        DefeitoNaoConformidades =
            agregacaoNaoConformidade.DefeitoNaoConformidades.ConvertAll(e =>
                new DefeitoNaoConformidadeOutput(e));
        
        SolucaoNaoConformidades =
            agregacaoNaoConformidade.SolucaoNaoConformidades.ConvertAll(e =>
                new SolucaoNaoConformidadeOutput(e));
        
        ProdutoNaoConformidades =
            agregacaoNaoConformidade.ProdutoNaoConformidades.ConvertAll(e =>
                new ProdutoNaoConformidadeOutput(e));
        
        ServicoNaoConformidades =
            agregacaoNaoConformidade.ServicoNaoConformidades.ConvertAll(e =>
                new ServicoNaoConformidadeOutput(e));
        
        CentroCustoCausaNaoConformidades =
            agregacaoNaoConformidade.CentroCustoCausaNaoConformidades.ConvertAll(e =>
                new CentroCustoCausaNaoConformidadeOutput(e));

    }
}
