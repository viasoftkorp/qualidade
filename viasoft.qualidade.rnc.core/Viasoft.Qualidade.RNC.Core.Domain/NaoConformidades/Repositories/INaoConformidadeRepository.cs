using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories
{
    public interface INaoConformidadeRepository
    {
        Task<AgregacaoNaoConformidade> Get(Guid id);
        Task<List<AgregacaoNaoConformidade>> GetList(List<NaoConformidade> naoConformidades);

        Task Update(UpdateNaoConformidadeInput input);

        Task Create(CreateNaoConformidadeInput input);

        Task Delete(NaoConformidade naoConformidade, ConclusaoNaoConformidade conclusao,
            ReclamacaoNaoConformidade reclamacao,
            List<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
            List<CausaNaoConformidade> causaNaoConformidades,
            List<DefeitoNaoConformidade> defeitoNaoConformidades, List<SolucaoNaoConformidade> solucaoNaoConformidades,
            List<ProdutoNaoConformidade> produtoNaoConformidades,
            List<ServicoNaoConformidade> servicoNaoConformidades);

        public INaoConformidadeRepository Operacoes();
    }

    public class CreateNaoConformidadeInput
    {
        public NaoConformidade NaoConformidadeACriar { get; set; }
        public ConclusaoNaoConformidade ConclusaoACriar { get; set; }
        public ReclamacaoNaoConformidade ReclamacaoACriar { get; set; }
        public List<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidadesACriar { get; set; }
        public List<CausaNaoConformidade> CausaNaoConformidadesACriar { get; set; }
        public List<DefeitoNaoConformidade> DefeitoNaoConformidadesACriar { get; set; }
        public List<SolucaoNaoConformidade> SolucaoNaoConformidadesACriar { get; set; }
        public List<ProdutoNaoConformidade> ProdutoNaoConformidadesACriar { get; set; }
        public List<ServicoNaoConformidade> ServicoNaoConformidadesACriar { get; set; }
    }

    public class UpdateNaoConformidadeInput
    {
        public NaoConformidade NaoConformidadeAtualizar { get; set; }
        public NaoConformidade NaoConformidadeRemover { get; set; }
        public ConclusaoNaoConformidade ConclusaoAtualizar { get; set; }
        public ConclusaoNaoConformidade ConclusaoCriar { get; set; }
        public ConclusaoNaoConformidade ConclusaoRemover { get; set; }
        public ReclamacaoNaoConformidade ReclamacaoAtualizar { get; set; }
        public ReclamacaoNaoConformidade ReclamacaoCriar { get; set; }
        public List<AcaoPreventivaNaoConformidade> AcoesAtualizar { get; set; }
        public List<AcaoPreventivaNaoConformidade> AcoesCriar { get; set; }
        public List<AcaoPreventivaNaoConformidade> AcoesRemover { get; set; }
        public List<CausaNaoConformidade> CausasAtualizar { get; set; }
        public List<CausaNaoConformidade> CausasCriar { get; set; }
        public List<CausaNaoConformidade> CausasRemover { get; set; }
        public List<DefeitoNaoConformidade> DefeitosAtualizar { get; set; }
        public List<DefeitoNaoConformidade> DefeitosCriar { get; set; }
        public List<DefeitoNaoConformidade> DefeitosRemover { get; set; }
        public List<SolucaoNaoConformidade> SolucoesAtualizar { get; set; }
        public List<SolucaoNaoConformidade> SolucoesCriar { get; set; }
        public List<SolucaoNaoConformidade> SolucoesRemover { get; set; }
        public List<ProdutoNaoConformidade> ProdutosAtualizar { get; set; }
        public List<ProdutoNaoConformidade> ProdutosCriar { get; set; }
        public List<ProdutoNaoConformidade> ProdutosRemover { get; set; }
        public List<ServicoNaoConformidade> ServicosAtualizar { get; set; }
        public List<ServicoNaoConformidade> ServicosCriar { get; set; }
        public List<ServicoNaoConformidade> ServicosRemover { get; set; }
        public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplemetacaoEvitarReincidenciaAAtualizar { get; set; }
        public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplemetacaoEvitarReincidenciaACriar { get; set; }
        public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplemetacaoEvitarReincidenciaARemover { get; set; }
        public List<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidadeCriar { get; set; }
        public List<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidadeRemover { get; set; }
    }
}
