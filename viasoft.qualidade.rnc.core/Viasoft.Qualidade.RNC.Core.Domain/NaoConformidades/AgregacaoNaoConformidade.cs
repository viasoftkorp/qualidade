using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.AcoesPreventivasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.CausasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ConclusaoNaoComformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.DefeitosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ProdutosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.ServicosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.SolucoesNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models.CausasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;

public class AgregacaoNaoConformidade
{
    public NaoConformidade NaoConformidade { get; init; }
    public NaoConformidade NaoConformidadeInserir { get; set; }
    public NaoConformidade NaoConformidadeAlterar { get; set; }
    public NaoConformidade NaoConformidadeRemover { get; set; }
    public ConclusaoNaoConformidade ConclusaoNaoConformidade { get; set; }
    public ConclusaoNaoConformidade ConclusaoNaoConformidadeInserir { get; set; }
    public ConclusaoNaoConformidade ConclusaoNaoConformidadeRemover { get; set; }
    public ConclusaoNaoConformidade ConclusaoNaoConformidadeAlterar { get; set; }
    public ReclamacaoNaoConformidade ReclamacaoNaoConformidade { get; init; }
    public ReclamacaoNaoConformidade ReclamacaoNaoConformidadeInserir { get; set; }
    public ReclamacaoNaoConformidade ReclamacaoNaoConformidadeAlterar { get; set; }
    public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidencia { get; init; }
    public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidenciaAInserir { get; init; }
    public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidenciaAAlterar { get; init; }
    public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidenciaARemover { get; init; }
    public List<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidades { get; init; }
    public List<AcaoPreventivaNaoConformidade> AcaoPreventivaInserir { get; init; }
    public List<AcaoPreventivaNaoConformidade> AcaoPreventivaAlterar { get; init; }
    public List<AcaoPreventivaNaoConformidade> AcaoPreventivaRemover { get; init; }
    public List<CausaNaoConformidade> CausaNaoConformidades { get; init; }
    public List<CausaNaoConformidade> CausaInserir { get; init; }
    public List<CausaNaoConformidade> CausaAlterar { get; init; }
    public List<CausaNaoConformidade> CausaRemover { get; init; }
    public List<DefeitoNaoConformidade> DefeitoNaoConformidades { get; init; }
    public List<DefeitoNaoConformidade> DefeitoInserir { get; init; }
    public List<DefeitoNaoConformidade> DefeitoAlterar { get; init; }
    public List<DefeitoNaoConformidade> DefeitoRemover { get; init; }
    public List<SolucaoNaoConformidade> SolucaoNaoConformidades { get; init; }
    public List<SolucaoNaoConformidade> SolucaoInserir { get; init; }
    public List<SolucaoNaoConformidade> SolucaoAlterar { get; init; }
    public List<SolucaoNaoConformidade> SolucaoRemover { get; init; }
    public List<ProdutoNaoConformidade> ProdutoNaoConformidades { get; init; }
    public List<ProdutoNaoConformidade> ProdutosInserir { get; init; }
    public List<ProdutoNaoConformidade> ProdutosAlterar { get; init; }
    public List<ProdutoNaoConformidade> ProdutosRemover { get; init; }
    public List<ServicoNaoConformidade> ServicoNaoConformidades { get; init; }
    public List<ServicoNaoConformidade> ServicoInserir { get; init; }
    public List<ServicoNaoConformidade> ServicoAlterar { get; init; }
    public List<ServicoNaoConformidade> ServicoRemover { get; init; }

    public List<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidades { get; init; }
    public List<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidadesInserir { get; init; }
    public List<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidadesRemover { get; init; }
    public List<BaseEvent> DomainEvents { get; set; }
    public OrdemRetrabalhoNaoConformidade OrdemRetrabalhoNaoConformidade { get; set; }

    public AgregacaoNaoConformidade()
    {
        AcaoPreventivaNaoConformidades = new List<AcaoPreventivaNaoConformidade>();
        AcaoPreventivaInserir = new List<AcaoPreventivaNaoConformidade>();
        AcaoPreventivaAlterar = new List<AcaoPreventivaNaoConformidade>();
        AcaoPreventivaRemover = new List<AcaoPreventivaNaoConformidade>();
        ImplementacaoEvitarReincidencia = new List<ImplementacaoEvitarReincidenciaNaoConformidade>();
        ImplementacaoEvitarReincidenciaAInserir = new List<ImplementacaoEvitarReincidenciaNaoConformidade>();
        ImplementacaoEvitarReincidenciaAAlterar = new List<ImplementacaoEvitarReincidenciaNaoConformidade>();
        ImplementacaoEvitarReincidenciaARemover = new List<ImplementacaoEvitarReincidenciaNaoConformidade>();
        CausaNaoConformidades = new List<CausaNaoConformidade>();
        CausaInserir = new List<CausaNaoConformidade>();
        CausaAlterar = new List<CausaNaoConformidade>();
        CausaRemover = new List<CausaNaoConformidade>();
        DefeitoNaoConformidades = new List<DefeitoNaoConformidade>();
        DefeitoInserir = new List<DefeitoNaoConformidade>();
        DefeitoAlterar = new List<DefeitoNaoConformidade>();
        DefeitoRemover = new List<DefeitoNaoConformidade>();
        SolucaoNaoConformidades = new List<SolucaoNaoConformidade>();
        SolucaoInserir = new List<SolucaoNaoConformidade>();
        SolucaoAlterar = new List<SolucaoNaoConformidade>();
        SolucaoRemover = new List<SolucaoNaoConformidade>();
        ProdutoNaoConformidades = new List<ProdutoNaoConformidade>();
        ProdutosInserir = new List<ProdutoNaoConformidade>();
        ProdutosAlterar = new List<ProdutoNaoConformidade>();
        ProdutosRemover = new List<ProdutoNaoConformidade>();
        ServicoNaoConformidades = new List<ServicoNaoConformidade>();
        ServicoInserir = new List<ServicoNaoConformidade>();
        ServicoAlterar = new List<ServicoNaoConformidade>();
        ServicoRemover = new List<ServicoNaoConformidade>();
        CentroCustoCausaNaoConformidades = new List<CentroCustoCausaNaoConformidade>();
        CentroCustoCausaNaoConformidadesInserir = new List<CentroCustoCausaNaoConformidade>();
        CentroCustoCausaNaoConformidadesRemover = new List<CentroCustoCausaNaoConformidade>();
        DomainEvents = new List<BaseEvent>();
    }

    public AgregacaoNaoConformidade(NaoConformidade naoConformidade,
        List<AcaoPreventivaNaoConformidade> acoesPreventivasNaoConformidades,
        List<CausaNaoConformidade> causasNaoConformidades,
        List<DefeitoNaoConformidade> defeitosNaoConformidades,
        List<SolucaoNaoConformidade> solucoesNaoConformidades,
        List<ProdutoNaoConformidade> produtosNaoConformidades,
        List<ServicoNaoConformidade> servicosNaoConformidades,
        List<CentroCustoCausaNaoConformidade> centroCustoCausaNaoConformidades,
        ConclusaoNaoConformidade conclusaoNaoConformidade,
        ReclamacaoNaoConformidade reclamacaoNaoConformidade,
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade,
        List<ImplementacaoEvitarReincidenciaNaoConformidade> implementacaoEvitarReincidenciaNaoConformidade) : this()
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
        OrdemRetrabalhoNaoConformidade = ordemRetrabalhoNaoConformidade;
    }

    public async Task CommitUpdate(AgregacaoNaoConformidade naoConformidade, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, INaoConformidadeRepository naoConformidadeRepository)
    {
        using (unitOfWork.Begin())
        {
            await naoConformidade.Update(naoConformidadeRepository);
            foreach (var domainEvent in naoConformidade.DomainEvents)
            {
                await serviceBus.Publish(domainEvent);
            }

            await unitOfWork.CompleteAsync();
        }
    }

    public Task Update(INaoConformidadeRepository repository)
    {
        var input = new UpdateNaoConformidadeInput
        {
            NaoConformidadeAtualizar = NaoConformidadeAlterar,
            NaoConformidadeRemover = NaoConformidadeRemover,
            ConclusaoAtualizar = ConclusaoNaoConformidadeAlterar,
            ConclusaoCriar = ConclusaoNaoConformidadeInserir,
            ConclusaoRemover = ConclusaoNaoConformidadeRemover,
            ReclamacaoAtualizar = ReclamacaoNaoConformidadeAlterar,
            ReclamacaoCriar = ReclamacaoNaoConformidadeInserir,
            AcoesAtualizar = AcaoPreventivaAlterar,
            AcoesCriar = AcaoPreventivaInserir,
            AcoesRemover = AcaoPreventivaRemover,
            CausasAtualizar = CausaAlterar,
            CausasCriar = CausaInserir,
            CausasRemover = CausaRemover,
            DefeitosAtualizar = DefeitoAlterar,
            DefeitosCriar = DefeitoInserir,
            DefeitosRemover = DefeitoRemover,
            SolucoesAtualizar = SolucaoAlterar,
            SolucoesCriar = SolucaoInserir,
            SolucoesRemover = SolucaoRemover,
            ProdutosAtualizar = ProdutosAlterar,
            ProdutosCriar = ProdutosInserir,
            ProdutosRemover = ProdutosRemover,
            ServicosAtualizar = ServicoAlterar,
            ServicosCriar = ServicoInserir,
            ServicosRemover = ServicoRemover,
            ImplemetacaoEvitarReincidenciaAAtualizar = ImplementacaoEvitarReincidenciaAAlterar,
            ImplemetacaoEvitarReincidenciaACriar = ImplementacaoEvitarReincidenciaAInserir,
            ImplemetacaoEvitarReincidenciaARemover = ImplementacaoEvitarReincidenciaARemover,
            CentroCustoCausaNaoConformidadeCriar = CentroCustoCausaNaoConformidadesInserir,
            CentroCustoCausaNaoConformidadeRemover = CentroCustoCausaNaoConformidadesRemover
        };
        return repository.Update(input);
    }

    public async Task SalvarNaoConformidade(INaoConformidadeRepository repository)
    {
        var input = new CreateNaoConformidadeInput
        {
            NaoConformidadeACriar = NaoConformidadeInserir,
            ConclusaoACriar = ConclusaoNaoConformidade,
            ReclamacaoACriar = ReclamacaoNaoConformidade,
            AcaoPreventivaNaoConformidadesACriar = AcaoPreventivaNaoConformidades,
            CausaNaoConformidadesACriar = CausaNaoConformidades,
            DefeitoNaoConformidadesACriar = DefeitoNaoConformidades,
            SolucaoNaoConformidadesACriar = SolucaoNaoConformidades,
            ProdutoNaoConformidadesACriar = ProdutoNaoConformidades,
            ServicoNaoConformidadesACriar = ServicoNaoConformidades
        };
        await repository.Create(input);
    }

    public async Task DeletarNaoConformidade(INaoConformidadeRepository repository)
    {
        await repository.Delete(NaoConformidade, ConclusaoNaoConformidade,
            ReclamacaoNaoConformidade,
            AcaoPreventivaNaoConformidades, CausaNaoConformidades, DefeitoNaoConformidades, SolucaoNaoConformidades,
            ProdutoNaoConformidades, ServicoNaoConformidades);
    }

    //AcaoPreventiva
    public void Process(AlterarAcaoPreventivaCommand acaoPreventivaCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var acaoPreventivaAlterada =
            new AcaoPreventivaNaoConformidadeAtualizada(dateTimeProvider, tenantId, environmentId)
            {
                Command = acaoPreventivaCommand
            };
        Apply(acaoPreventivaAlterada);
        DomainEvents.Add(acaoPreventivaAlterada);
    }

    private void Apply(AcaoPreventivaNaoConformidadeAtualizada acaoPreventivaNaoConformidadeAlterada)
    {
        var acaoAAtualizar =
            AcaoPreventivaNaoConformidades.First(p =>
                p.Id == acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.Id);
        acaoAAtualizar.Acao = acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.Acao;
        acaoAAtualizar.Detalhamento =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.Detalhamento;
        acaoAAtualizar.Implementada =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.Implementada;
        acaoAAtualizar.DataAnalise =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.DataAnalise;
        acaoAAtualizar.DataVerificacao = acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade
            .DataVerificacao;
        acaoAAtualizar.IdAuditor =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.IdAuditor;
        acaoAAtualizar.NovaData = acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.NovaData;
        acaoAAtualizar.IdResponsavel =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.IdResponsavel;
        acaoAAtualizar.DataPrevistaImplantacao =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.DataPrevistaImplantacao;
        acaoAAtualizar.CompanyId =
            acaoPreventivaNaoConformidadeAlterada.Command.AcaoPreventivaNaoConformidade.CompanyId;
        AcaoPreventivaAlterar.Add(acaoAAtualizar);
    }

    public void Process(InserirAcaoPreventivaCommand acaoPreventivaCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var acaoPreventivaInserida =
            new AcaoPreventivaNaoConformidadeInserida(dateTimeProvider, tenantId, environmentId)
            {
                IdNaoConformidade = NaoConformidade.Id,
                Command = acaoPreventivaCommand
            };
        Apply(acaoPreventivaInserida);
        DomainEvents.Add(acaoPreventivaInserida);
    }

    private void Apply(AcaoPreventivaNaoConformidadeInserida acaoPreventivaNaoConformidadeInserida)
    {
        var acaoAInserir = new AcaoPreventivaNaoConformidade()
        {
            IdNaoConformidade = acaoPreventivaNaoConformidadeInserida.IdNaoConformidade,
            Id = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.Id,
            IdDefeitoNaoConformidade = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade
                .IdDefeitoNaoConformidade,
            IdAcaoPreventiva = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade
                .IdAcaoPreventiva,
            Acao = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.Acao,
            Detalhamento = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.Detalhamento,
            Implementada = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.Implementada,
            DataAnalise = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.DataAnalise,
            DataVerificacao = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade
                .DataVerificacao,
            IdAuditor = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.IdAuditor,
            NovaData = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.NovaData,
            IdResponsavel = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.IdResponsavel,
            DataPrevistaImplantacao = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade
                .DataPrevistaImplantacao,
            CompanyId = acaoPreventivaNaoConformidadeInserida.Command.AcaoPreventivaNaoConformidade.CompanyId
        };
        AcaoPreventivaInserir.Add(acaoAInserir);
        AcaoPreventivaNaoConformidades.Add(acaoAInserir);
    }

    public void Process(RemoverAcaoPreventivaCommand acaoPreventivaCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var acaoPreventivaRemovida =
            new AcaoPreventivaNaoConformidadeRemovida(dateTimeProvider, tenantId, environmentId)
            {
                IdAcaoPreventiva = acaoPreventivaCommand.Id
            };
        Apply(acaoPreventivaRemovida);
        DomainEvents.Add(acaoPreventivaRemovida);
    }

    private void Apply(AcaoPreventivaNaoConformidadeRemovida acaoPreventivaNaoConformidadeRemovida)
    {
        var acaoARemover =
            AcaoPreventivaNaoConformidades.First(item =>
                item.Id.Equals(acaoPreventivaNaoConformidadeRemovida.IdAcaoPreventiva));
        AcaoPreventivaRemover.Add(acaoARemover);
        AcaoPreventivaNaoConformidades.Remove(acaoARemover);
    }

    //NaoConformidade
    public void Process(InserirNaoConformidadeCommand inserirCommand, IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId, Guid userId)
    {
        inserirCommand.NaoConformidade.IdCriador = userId;
        inserirCommand.NaoConformidade.DataCriacao = dateTimeProvider.UtcNow();

        var naoConformidade = new NaoConformidadeInserida(dateTimeProvider, tenantId, environmentId)
        {
            NaoConformidade = inserirCommand.NaoConformidade
        };

        Apply(naoConformidade);
        DomainEvents.Add(naoConformidade);
    }

    private void Apply(NaoConformidadeInserida naoConformidadeInserida)
    {
        var naoConformidadeAInserir = new NaoConformidade();

        naoConformidadeAInserir.Codigo = naoConformidadeInserida.NaoConformidade.Codigo;
        naoConformidadeAInserir.Descricao = naoConformidadeInserida.NaoConformidade.Descricao;
        naoConformidadeAInserir.Equipe = naoConformidadeInserida.NaoConformidade.Equipe;
        naoConformidadeAInserir.Id = naoConformidadeInserida.NaoConformidade.Id;
        naoConformidadeAInserir.Origem = naoConformidadeInserida.NaoConformidade.Origem;
        naoConformidadeAInserir.Rejeitado = naoConformidadeInserida.NaoConformidade.Rejeitado;
        naoConformidadeAInserir.Revisao = naoConformidadeInserida.NaoConformidade.Revisao;
        naoConformidadeAInserir.Status = naoConformidadeInserida.NaoConformidade.Status;
        naoConformidadeAInserir.AceitoConcessao = naoConformidadeInserida.NaoConformidade.AceitoConcessao;
        naoConformidadeAInserir.CampoNf = naoConformidadeInserida.NaoConformidade.CampoNf;
        naoConformidadeAInserir.IdPessoa = naoConformidadeInserida.NaoConformidade.IdPessoa;
        naoConformidadeAInserir.IdCriador = naoConformidadeInserida.NaoConformidade.IdCriador;
        naoConformidadeAInserir.NumeroLote = naoConformidadeInserida.NaoConformidade.NumeroLote;
        naoConformidadeAInserir.IdNatureza = naoConformidadeInserida.NaoConformidade.IdNatureza;
        naoConformidadeAInserir.NumeroOdf = naoConformidadeInserida.NaoConformidade.NumeroOdf;
        naoConformidadeAInserir.IdProduto = naoConformidadeInserida.NaoConformidade.IdProduto;
        naoConformidadeAInserir.LoteParcial = naoConformidadeInserida.NaoConformidade.LoteParcial;
        naoConformidadeAInserir.LoteTotal = naoConformidadeInserida.NaoConformidade.LoteTotal;
        naoConformidadeAInserir.DataFabricacaoLote = naoConformidadeInserida.NaoConformidade.DataFabricacaoLote;
        naoConformidadeAInserir.NumeroNotaFiscal = naoConformidadeInserida.NaoConformidade.NumeroNotaFiscal;
        naoConformidadeAInserir.IdNotaFiscal = naoConformidadeInserida.NaoConformidade.IdNotaFiscal;
        naoConformidadeAInserir.MelhoriaEmPotencial = naoConformidadeInserida.NaoConformidade.MelhoriaEmPotencial;
        naoConformidadeAInserir.RelatoNaoConformidade = naoConformidadeInserida.NaoConformidade.RelatoNaoConformidade;
        naoConformidadeAInserir.RetrabalhoNoCliente = naoConformidadeInserida.NaoConformidade.RetrabalhoNoCliente;
        naoConformidadeAInserir.RetrabalhoPeloCliente = naoConformidadeInserida.NaoConformidade.RetrabalhoPeloCliente;
        naoConformidadeAInserir.NaoConformidadeEmPotencial =
            naoConformidadeInserida.NaoConformidade.NaoConformidadeEmPotencial;
        naoConformidadeAInserir.NumeroPedido = naoConformidadeInserida.NaoConformidade.NumeroPedido;
        naoConformidadeAInserir.NumeroOdfFaturamento = naoConformidadeInserida.NaoConformidade.NumeroOdfFaturamento;
        naoConformidadeAInserir.IdProdutoFaturamento = naoConformidadeInserida.NaoConformidade.IdProdutoFaturamento;
        naoConformidadeAInserir.CompanyId = naoConformidadeInserida.NaoConformidade.CompanyId;
        naoConformidadeAInserir.Incompleta = naoConformidadeInserida.NaoConformidade.Incompleta;
        naoConformidadeAInserir.DataCriacao = naoConformidadeInserida.NaoConformidade.DataCriacao;
        NaoConformidadeInserir = naoConformidadeAInserir;
    }

    public void Process(AlterarNaoConformidadeCommand alterarCommand, IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var naoConformidade = new NaoConformidadeAtualizada(dateTimeProvider, tenantId, environmentId)
        {
            NaoConformidade = alterarCommand.NaoConformidade
        };
        Apply(naoConformidade);
        DomainEvents.Add(naoConformidade);
    }

    private void Apply(NaoConformidadeAtualizada naoConformidadeAtualizada)
    {
        var naoConformidadeAAtualizar = NaoConformidade;

        naoConformidadeAAtualizar.Descricao = naoConformidadeAtualizada.NaoConformidade.Descricao;
        naoConformidadeAAtualizar.Equipe = naoConformidadeAtualizada.NaoConformidade.Equipe;
        naoConformidadeAAtualizar.Origem = naoConformidadeAtualizada.NaoConformidade.Origem;
        naoConformidadeAAtualizar.Rejeitado = naoConformidadeAtualizada.NaoConformidade.Rejeitado;
        naoConformidadeAAtualizar.Revisao = naoConformidadeAtualizada.NaoConformidade.Revisao;
        naoConformidadeAAtualizar.Status = naoConformidadeAtualizada.NaoConformidade.Status;
        naoConformidadeAAtualizar.AceitoConcessao = naoConformidadeAtualizada.NaoConformidade.AceitoConcessao;
        naoConformidadeAAtualizar.CampoNf = naoConformidadeAtualizada.NaoConformidade.CampoNf;
        naoConformidadeAAtualizar.IdPessoa = naoConformidadeAtualizada.NaoConformidade.IdPessoa;
        naoConformidadeAAtualizar.IdCriador = NaoConformidade.IdCriador;
        naoConformidadeAAtualizar.NumeroLote = naoConformidadeAtualizada.NaoConformidade.NumeroLote;
        naoConformidadeAAtualizar.IdNatureza = naoConformidadeAtualizada.NaoConformidade.IdNatureza;
        naoConformidadeAAtualizar.NumeroOdf = naoConformidadeAtualizada.NaoConformidade.NumeroOdf;
        naoConformidadeAAtualizar.IdProduto = naoConformidadeAtualizada.NaoConformidade.IdProduto;
        naoConformidadeAAtualizar.LoteParcial = naoConformidadeAtualizada.NaoConformidade.LoteParcial;
        naoConformidadeAAtualizar.LoteTotal = naoConformidadeAtualizada.NaoConformidade.LoteTotal;
        naoConformidadeAAtualizar.DataFabricacaoLote = naoConformidadeAtualizada.NaoConformidade.DataFabricacaoLote;
        naoConformidadeAAtualizar.NumeroNotaFiscal = naoConformidadeAtualizada.NaoConformidade.NumeroNotaFiscal;
        naoConformidadeAAtualizar.IdNotaFiscal = naoConformidadeAtualizada.NaoConformidade.IdNotaFiscal;
        naoConformidadeAAtualizar.MelhoriaEmPotencial = naoConformidadeAtualizada.NaoConformidade.MelhoriaEmPotencial;
        naoConformidadeAAtualizar.RelatoNaoConformidade =
            naoConformidadeAtualizada.NaoConformidade.RelatoNaoConformidade;
        naoConformidadeAAtualizar.RetrabalhoNoCliente = naoConformidadeAtualizada.NaoConformidade.RetrabalhoNoCliente;
        naoConformidadeAAtualizar.RetrabalhoPeloCliente =
            naoConformidadeAtualizada.NaoConformidade.RetrabalhoPeloCliente;
        naoConformidadeAAtualizar.NaoConformidadeEmPotencial =
            naoConformidadeAtualizada.NaoConformidade.NaoConformidadeEmPotencial;
        naoConformidadeAAtualizar.NumeroPedido = naoConformidadeAtualizada.NaoConformidade.NumeroPedido;
        naoConformidadeAAtualizar.NumeroOdfFaturamento = naoConformidadeAtualizada.NaoConformidade.NumeroOdfFaturamento;
        naoConformidadeAAtualizar.IdProdutoFaturamento = naoConformidadeAtualizada.NaoConformidade.IdProdutoFaturamento;
        naoConformidadeAAtualizar.CompanyId = naoConformidadeAtualizada.NaoConformidade.CompanyId;
        naoConformidadeAAtualizar.Incompleta = naoConformidadeAtualizada.NaoConformidade.Incompleta;

        NaoConformidadeAlterar = naoConformidadeAAtualizar;
    }

    public void Process(RemoverNaoConformidadeCommand removerCommand, IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var naoConformidade = new NaoConformidadeRemovida(dateTimeProvider, tenantId, environmentId)
        {
            IdNaoConformidade = removerCommand.Id
        };
        Apply(naoConformidade);
        DomainEvents.Add(naoConformidade);
    }

    private void Apply(NaoConformidadeRemovida naoConformidadeRemovida)
    {
        NaoConformidadeRemover = NaoConformidade;
    }

    //Defeito
    public void Process(AlterarDefeitoCommand defeitoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var defeitoAtualizado = new DefeitoNaoConformidadeAtualizado(dateTimeProvider, tenantId, environmentId)
        {
            Command = defeitoCommand
        };
        Apply(defeitoAtualizado);
        DomainEvents.Add(defeitoAtualizado);
    }

    private void Apply(DefeitoNaoConformidadeAtualizado defeitoNaoConformidadeAtualizado)
    {
        var defeitoAAtualizar =
            DefeitoNaoConformidades.First(p =>
                p.Id == defeitoNaoConformidadeAtualizado.Command.DefeitoNaoConformidade.Id);

        defeitoNaoConformidadeAtualizado.IdDefeitoAnterior = defeitoAAtualizar.IdDefeito;
        defeitoAAtualizar.IdDefeito = defeitoNaoConformidadeAtualizado.Command.DefeitoNaoConformidade.IdDefeito;
        defeitoAAtualizar.Quantidade = defeitoNaoConformidadeAtualizado.Command.DefeitoNaoConformidade.Quantidade;
        defeitoAAtualizar.Detalhamento = defeitoNaoConformidadeAtualizado.Command.DefeitoNaoConformidade.Detalhamento;
        defeitoAAtualizar.CompanyId = defeitoNaoConformidadeAtualizado.Command.DefeitoNaoConformidade.CompanyId;

        DefeitoAlterar.Add(defeitoAAtualizar);
    }

    public void Process(InserirDefeitoCommand defeitoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var defeitoInserido = new DefeitoNaoConformidadeInserido(dateTimeProvider, tenantId, environmentId)
        {
            IdNaoConformidade = NaoConformidade.Id,
            Command = defeitoCommand
        };
        Apply(defeitoInserido);
        DomainEvents.Add(defeitoInserido);
    }

    private void Apply(DefeitoNaoConformidadeInserido defeitoNaoConformidadeInserido)
    {
        var defeitoAInserir = new DefeitoNaoConformidade()
        {
            IdNaoConformidade = defeitoNaoConformidadeInserido.IdNaoConformidade,
            IdDefeito = defeitoNaoConformidadeInserido.Command.DefeitoNaoConformidade.IdDefeito,
            Id = defeitoNaoConformidadeInserido.Command.DefeitoNaoConformidade.Id,
            Detalhamento = defeitoNaoConformidadeInserido.Command.DefeitoNaoConformidade.Detalhamento,
            Quantidade = defeitoNaoConformidadeInserido.Command.DefeitoNaoConformidade.Quantidade,
            CompanyId = defeitoNaoConformidadeInserido.Command.DefeitoNaoConformidade.CompanyId
        };
        DefeitoInserir.Add(defeitoAInserir);
        DefeitoNaoConformidades.Add(defeitoAInserir);
    }

    public void Process(RemoverDefeitoCommand defeitoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var defeitoRemovido = new DefeitoNaoConformidadeRemovido(dateTimeProvider, tenantId, environmentId)
        {
            IdDefeito = defeitoCommand.Id
        };
        Apply(defeitoRemovido);
        DomainEvents.Add(defeitoRemovido);
    }

    private void Apply(DefeitoNaoConformidadeRemovido defeitoNaoConformidadeRemovido)
    {
        var defeitoARemover =
            DefeitoNaoConformidades.First(entity => entity.Id.Equals(defeitoNaoConformidadeRemovido.IdDefeito));
        DefeitoRemover.Add(defeitoARemover);
        DefeitoNaoConformidades.Remove(defeitoARemover);
    }

    //Causa
    public void Process(AlterarCausaCommand causaCommand, IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId, Guid companyId)
    {
        var causaAtualizada = new CausaNaoConformidadeAtualizada(dateTimeProvider, tenantId, environmentId)
        {
            Command = causaCommand
        };

        Apply(causaAtualizada);
        DomainEvents.Add(causaAtualizada);

        InserirCentrosCustoQueNaoEstaoNaAgregacao(causaAtualizada.Command.CausaNaoConformidade,
            dateTimeProvider, tenantId, environmentId, companyId);
        RemoverCentrosCustoQueNaoEstaoNoInput(causaAtualizada.Command.CausaNaoConformidade,
            dateTimeProvider, tenantId, environmentId);
    }

    private void Apply(CausaNaoConformidadeAtualizada causaNaoConformidadeAtualizada)
    {
        var causaAAtualizar =
            CausaNaoConformidades.First(p => p.Id == causaNaoConformidadeAtualizada.Command.CausaNaoConformidade.Id);
        causaAAtualizar.IdCausa = causaNaoConformidadeAtualizada.Command.CausaNaoConformidade.IdCausa;
        causaAAtualizar.Detalhamento = causaNaoConformidadeAtualizada.Command.CausaNaoConformidade.Detalhamento;
        causaAAtualizar.CompanyId = causaNaoConformidadeAtualizada.Command.CausaNaoConformidade.CompanyId;
        CausaAlterar.Add(causaAAtualizar);
    }

    public void Process(InserirCausaCommand causaCommand, IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId, Guid companyId)
    {
        var causaInserida = new CausaNaoConformidadeInserida(dateTimeProvider, tenantId, environmentId)
        {
            IdNaoConformidade = NaoConformidade.Id,
            Command = causaCommand
        };

        Apply(causaInserida);
        DomainEvents.Add(causaInserida);

        InserirCentrosCustoQueNaoEstaoNaAgregacao(causaInserida.Command.CausaNaoConformidade,
            dateTimeProvider, tenantId, environmentId, companyId);
    }

    private void Apply(CausaNaoConformidadeInserida causaNaoConformidadeInserida)
    {
        var causaAInserir = new CausaNaoConformidade
        {
            IdNaoConformidade = causaNaoConformidadeInserida.IdNaoConformidade,
            Detalhamento = causaNaoConformidadeInserida.Command.CausaNaoConformidade.Detalhamento,
            IdDefeitoNaoConformidade =
                causaNaoConformidadeInserida.Command.CausaNaoConformidade.IdDefeitoNaoConformidade,
            IdCausa = causaNaoConformidadeInserida.Command.CausaNaoConformidade.IdCausa,
            Id = causaNaoConformidadeInserida.Command.CausaNaoConformidade.Id,
            CompanyId = causaNaoConformidadeInserida.Command.CausaNaoConformidade.CompanyId,
        };
        CausaInserir.Add(causaAInserir);
        CausaNaoConformidades.Add(causaAInserir);
    }

    public void Process(RemoverCausaCommand causaCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var causaRemovida = new CausaNaoConformidadeRemovida(dateTimeProvider, tenantId, environmentId)
        {
            IdCausa = causaCommand.Id
        };
        Apply(causaRemovida);
        DomainEvents.Add(causaRemovida);
    }

    private void Apply(CausaNaoConformidadeRemovida causaNaoConformidadeRemovida)
    {
        var causaARemover =
            CausaNaoConformidades.First(causa => causa.Id.Equals(causaNaoConformidadeRemovida.IdCausa));
        CausaRemover.Add(causaARemover);
        CausaNaoConformidades.Remove(causaARemover);
    }

    //Solucao
    public void Process(AlterarSolucaoCommand solucaoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var solucaoAtualizada = new SolucaoNaoConformidadeAtualizada(dateTimeProvider, tenantId, environmentId)
        {
            Command = solucaoCommand
        };
        Apply(solucaoAtualizada);
    }

    private void Apply(SolucaoNaoConformidadeAtualizada solucaoNaoConformidadeAtualizada)
    {
        var solucaoAAtualizar =
            SolucaoNaoConformidades.First(p =>
                p.Id == solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.Id);
        solucaoNaoConformidadeAtualizada.IdSolucaoAnterior = solucaoAAtualizar.IdSolucao;
        solucaoAAtualizar.Id = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.Id;
        solucaoAAtualizar.CustoEstimado = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.CustoEstimado;
        solucaoAAtualizar.Detalhamento = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.Detalhamento;
        solucaoAAtualizar.IdSolucao = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.IdSolucao;
        solucaoAAtualizar.DataAnalise = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.DataAnalise;
        solucaoAAtualizar.DataVerificacao =
            solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.DataVerificacao;
        solucaoAAtualizar.IdAuditor = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.IdAuditor;
        solucaoAAtualizar.IdResponsavel = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.IdResponsavel;
        solucaoAAtualizar.NovaData = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.NovaData;
        solucaoAAtualizar.SolucaoImediata =
            solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.SolucaoImediata;
        solucaoAAtualizar.DataPrevistaImplantacao =
            solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.DataPrevistaImplantacao;
        solucaoAAtualizar.IdNaoConformidade =
            solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.IdNaoConformidade;
        solucaoAAtualizar.IdDefeitoNaoConformidade = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade
            .IdDefeitoNaoConformidade;
        solucaoAAtualizar.CompanyId = solucaoNaoConformidadeAtualizada.Command.SolucaoNaoConformidade.CompanyId;
        SolucaoAlterar.Add(solucaoAAtualizar);
    }

    public void Process(InserirSolucaoCommand solucaoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var solucaoInserida = new SolucaoNaoConformidadeInserida(dateTimeProvider, tenantId, environmentId)
        {
            IdNaoConformidade = NaoConformidade.Id,
            Command = solucaoCommand
        };
        Apply(solucaoInserida);
        DomainEvents.Add(solucaoInserida);
    }

    private void Apply(SolucaoNaoConformidadeInserida solucaoNaoConformidadeInserida)
    {
        var solucaoAInserir = new SolucaoNaoConformidade()
        {
            Id = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.Id,
            CustoEstimado = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.CustoEstimado,
            DataAnalise = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.DataAnalise,
            DataVerificacao = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.DataVerificacao,
            IdAuditor = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.IdAuditor,
            IdResponsavel = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.IdResponsavel,
            NovaData = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.NovaData,
            SolucaoImediata = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.SolucaoImediata,
            DataPrevistaImplantacao =
                solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.DataPrevistaImplantacao,
            IdNaoConformidade = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.IdNaoConformidade,
            IdDefeitoNaoConformidade =
                solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.IdDefeitoNaoConformidade,
            Detalhamento = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.Detalhamento,
            IdSolucao = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.IdSolucao,
            CompanyId = solucaoNaoConformidadeInserida.Command.SolucaoNaoConformidade.CompanyId
        };
        SolucaoInserir.Add(solucaoAInserir);
        SolucaoNaoConformidades.Add(solucaoAInserir);
    }

    public void Process(RemoverSolucaoCommand solucaoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var solucaoRemovida = new SolucaoNaoConformidadeRemovida(dateTimeProvider, tenantId, environmentId)
        {
            IdSolucaoNaoConformidade = solucaoCommand.Id
        };
        Apply(solucaoRemovida);
    }

    private void Apply(SolucaoNaoConformidadeRemovida solucaoNaoConformidadeRemovida)
    {
        var solucaoARemover =
            SolucaoNaoConformidades.First(solucao =>
                solucao.Id.Equals(solucaoNaoConformidadeRemovida.IdSolucaoNaoConformidade));
        SolucaoRemover.Add(solucaoARemover);
        SolucaoNaoConformidades.Remove(solucaoARemover);
    }

    //ProdutoSolucao
    public void Process(AlterarProdutoNaoConformidadeCommand produtoNaoConformidadeCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var produtoSolucaoAtualizado =
            new ProdutoNaoConformidadeAtualizado(dateTimeProvider, tenantId, environmentId)
            {
                Command = produtoNaoConformidadeCommand
            };
        Apply(produtoSolucaoAtualizado);
        DomainEvents.Add(produtoSolucaoAtualizado);
    }

    private void Apply(ProdutoNaoConformidadeAtualizado produtoNaoConformidadeAtualizado)
    {
        var produtoSolucaoAAtualizar =
            ProdutoNaoConformidades.First(p =>
                p.Id == produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.Id);

        produtoSolucaoAAtualizar.Id = produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.Id;
        produtoSolucaoAAtualizar.Quantidade =
            produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.Quantidade;
        produtoSolucaoAAtualizar.Detalhamento =
            produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.Detalhamento;
        produtoSolucaoAAtualizar.IdProduto = produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.IdProduto;
        produtoSolucaoAAtualizar.IdNaoConformidade =
            produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.IdNaoConformidade;
        produtoSolucaoAAtualizar.OperacaoEngenharia =
            produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.OperacaoEngenharia;
        produtoSolucaoAAtualizar.CompanyId = produtoNaoConformidadeAtualizado.Command.ProdutoNaoConformidade.CompanyId;

        ProdutosAlterar.Add(produtoSolucaoAAtualizar);
    }

    public void Process(InserirProdutoNaoConformidadeCommand produtoNaoConformidadeCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var produtoSolucaoInserido =
            new ProdutoNaoConformidadeInserido(dateTimeProvider, tenantId, environmentId)
            {
                IdNaoConformidade = NaoConformidade.Id,
                Command = produtoNaoConformidadeCommand
            };
        Apply(produtoSolucaoInserido);
        DomainEvents.Add(produtoSolucaoInserido);
    }

    private void Apply(ProdutoNaoConformidadeInserido produtoNaoConformidadeInserido)
    {
        var produtoSolucaoAInserir = new ProdutoNaoConformidade
        {
            Id = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.Id,
            Quantidade = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.Quantidade,
            IdProduto = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.IdProduto,
            IdNaoConformidade = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.IdNaoConformidade,
            Detalhamento = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.Detalhamento,
            OperacaoEngenharia = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.OperacaoEngenharia,
            CompanyId = produtoNaoConformidadeInserido.Command.ProdutoNaoConformidade.CompanyId,
        };
        ProdutosInserir.Add(produtoSolucaoAInserir);
        ProdutoNaoConformidades.Add(produtoSolucaoAInserir);
    }

    public void Process(RemoverProdutoNaoConformidadeCommand produtoNaoConformidadeCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var produtoSolucaoRemovido =
            new ProdutoNaoConformidadeRemovido(dateTimeProvider, tenantId, environmentId)
            {
                IdProdutoSolucao = produtoNaoConformidadeCommand.Id
            };
        Apply(produtoSolucaoRemovido);
        DomainEvents.Add(produtoSolucaoRemovido);
    }

    private void Apply(ProdutoNaoConformidadeRemovido ProdutoNaoConformidadeRemovido)
    {
        var produtoSolucaoARemover =
            ProdutoNaoConformidades.First(solucao =>
                solucao.Id.Equals(ProdutoNaoConformidadeRemovido.IdProdutoSolucao));
        ProdutosRemover.Add(produtoSolucaoARemover);
        ProdutoNaoConformidades.Remove(produtoSolucaoARemover);
    }

    //ServicoSolucao
    public void Process(AlterarServicosNaoConformidadeCommand servicosNaoConformidadeCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var servicoSolucaoAtualizado =
            new ServicoNaoConformidadeAtualizado(dateTimeProvider, tenantId, environmentId)
            {
                Command = servicosNaoConformidadeCommand
            };
        Apply(servicoSolucaoAtualizado);
        DomainEvents.Add(servicoSolucaoAtualizado);
    }

    private void Apply(ServicoNaoConformidadeAtualizado servicoNaoConformidadeAtualizado)
    {
        var servicoSolucaoAAtualizar =
            ServicoNaoConformidades.First(p =>
                p.Id == servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.Id);
        servicoSolucaoAAtualizar.Id = servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.Id;
        servicoSolucaoAAtualizar.Quantidade =
            servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.Quantidade;
        servicoSolucaoAAtualizar.Detalhamento =
            servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.Detalhamento;
        servicoSolucaoAAtualizar.IdProduto = servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.IdProduto;
        servicoSolucaoAAtualizar.IdNaoConformidade =
            servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.IdNaoConformidade;
        servicoSolucaoAAtualizar.Horas = servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.Horas;
        servicoSolucaoAAtualizar.Minutos = servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.Minutos;
        servicoSolucaoAAtualizar.IdRecurso = servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.IdRecurso;
        servicoSolucaoAAtualizar.OperacaoEngenharia =
            servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.OperacaoEngenharia;
        servicoSolucaoAAtualizar.CompanyId = servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.CompanyId;
        servicoSolucaoAAtualizar.ControlarApontamento =
            servicoNaoConformidadeAtualizado.Command.ServicoNaoConformidade.ControlarApontamento;
        ServicoAlterar.Add(servicoSolucaoAAtualizar);
    }

    public void Process(InserirServicoNaoConformidadeCommand servicoNaoConformidadeCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var servicoSolucaoInserido =
            new ServicoNaoConformidadeInserido(dateTimeProvider, tenantId, environmentId)
            {
                IdNaoConformidade = NaoConformidade.Id,
                ServicoNaoConformidade = servicoNaoConformidadeCommand
            };
        Apply(servicoSolucaoInserido);
        DomainEvents.Add(servicoSolucaoInserido);
    }

    private void Apply(ServicoNaoConformidadeInserido servicoNaoConformidadeInserido)
    {
        var servicoSolucaoAInserir = new ServicoNaoConformidade()
        {
            Id = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.Id,
            Horas = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.Horas,
            Minutos = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.Minutos,
            IdRecurso = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.IdRecurso,
            OperacaoEngenharia = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade
                .OperacaoEngenharia,
            Quantidade = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.Quantidade,
            Detalhamento = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.Detalhamento,
            IdProduto = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.IdProduto,
            IdNaoConformidade = servicoNaoConformidadeInserido.IdNaoConformidade,
            CompanyId = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.CompanyId,
            ControlarApontamento = servicoNaoConformidadeInserido.ServicoNaoConformidade.ServicoNaoConformidade.ControlarApontamento
        };
        ServicoInserir.Add(servicoSolucaoAInserir);
        ServicoNaoConformidades.Add(servicoSolucaoAInserir);
    }

    public void Process(RemoverServicoNaoConformidadeCommand servicoNaoConformidadeCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var servicoSolucaoRemovido =
            new ServicoNaoConformidadeRemovido(dateTimeProvider, tenantId, environmentId)
            {
                IdServicoSolucao = servicoNaoConformidadeCommand.Id
            };
        Apply(servicoSolucaoRemovido);
        DomainEvents.Add(servicoSolucaoRemovido);
    }

    private void Apply(ServicoNaoConformidadeRemovido servicoNaoConformidadeRemovido)
    {
        var servicoSolucaoARemover =
            ServicoNaoConformidades.First(solucao =>
                solucao.Id.Equals(servicoNaoConformidadeRemovido.IdServicoSolucao));
        ServicoRemover.Add(servicoSolucaoARemover);
        ServicoNaoConformidades.Remove(servicoSolucaoARemover);
    }

    private void Apply(CentroCustoCausaNaoConformidadeInserido entidadeInserida)
    {
        var entidadeAInserir = new CentroCustoCausaNaoConformidade
        {
            Id = Guid.NewGuid(),
            IdCentroCusto = entidadeInserida.Command.CentroCustoCausaNaoConformidade.IdCentroCusto,
            IdNaoConformidade = entidadeInserida.Command.CentroCustoCausaNaoConformidade.IdNaoConformidade,
            IdCausaNaoConformidade = entidadeInserida.Command.CentroCustoCausaNaoConformidade.IdCausaNaoConformidade,
            CompanyId = entidadeInserida.Command.CentroCustoCausaNaoConformidade.CompanyId
        };
        CentroCustoCausaNaoConformidadesInserir.Add(entidadeAInserir);
        CentroCustoCausaNaoConformidades.Add(entidadeAInserir);
    }

    private void Apply(CentroCustoCausaNaoConformidadeRemovido entidadeRemovida)
    {
        var entidadeARemover =
            CentroCustoCausaNaoConformidades.First(entidade =>
                entidade.Id.Equals(entidadeRemovida.Id));
        CentroCustoCausaNaoConformidadesRemover.Add(entidadeARemover);
        CentroCustoCausaNaoConformidades.Remove(entidadeARemover);
    }

    //Conclusao
    public void Process(ConcluirNaoConformidadeCommand conclusaoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var naoConformidade = NaoConformidade;
        var conclusaoInserida = new ConclusaoInserida(dateTimeProvider, tenantId, environmentId)
        {
            Command = conclusaoCommand
        };
        Apply(conclusaoInserida, naoConformidade);
        DomainEvents.Add(conclusaoInserida);
    }

    public void Process(EstornarConclusaoCommand command,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var naoConformidade = NaoConformidade;
        var conclusaoEstornada = new ConclusaoEstornada(dateTimeProvider, tenantId, environmentId);
        Apply(naoConformidade);
        DomainEvents.Add(conclusaoEstornada);
    }

    private void Apply(ConclusaoInserida conclusaoInserida, NaoConformidade naoConformidade)
    {
        var conclusaoAInserir =
            new ConclusaoNaoConformidade();
        conclusaoAInserir.Id = conclusaoInserida.Command.ConclusaoNaoConformidade.Id;
        conclusaoAInserir.IdNaoConformidade = conclusaoInserida.Command.ConclusaoNaoConformidade.IdNaoConformidade;
        conclusaoAInserir.NovaReuniao = conclusaoInserida.Command.ConclusaoNaoConformidade.NovaReuniao;
        conclusaoAInserir.DataReuniao = conclusaoInserida.Command.ConclusaoNaoConformidade.DataReuniao;
        conclusaoAInserir.DataVerificacao = conclusaoInserida.Command.ConclusaoNaoConformidade.DataVerificacao;
        conclusaoAInserir.Evidencia = conclusaoInserida.Command.ConclusaoNaoConformidade.Evidencia;
        conclusaoAInserir.IdAuditor = conclusaoInserida.Command.ConclusaoNaoConformidade.IdAuditor;
        conclusaoAInserir.Eficaz = conclusaoInserida.Command.ConclusaoNaoConformidade.Eficaz;
        conclusaoAInserir.CicloDeTempo = conclusaoInserida.Command.ConclusaoNaoConformidade.CicloDeTempo;
        conclusaoAInserir.IdNovoRelatorio = conclusaoInserida.Command.ConclusaoNaoConformidade.IdNovoRelatorio;
        conclusaoAInserir.CompanyId = conclusaoInserida.Command.ConclusaoNaoConformidade.CompanyId;
        naoConformidade.Status = StatusNaoConformidade.Fechado;
        ConclusaoNaoConformidadeInserir = conclusaoAInserir;
    }
    private void Apply(NaoConformidade naoConformidade)
    {
        ConclusaoNaoConformidadeRemover = ConclusaoNaoConformidade;
        ConclusaoNaoConformidade = null;

        naoConformidade.Status = StatusNaoConformidade.Aberto;
    }

    // Reclamacao
    public void Process(InserirReclamacaoNaoConformidadeCommand inserirReclamacaoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var reclamacaoInserida = new ReclamacaoInserida(dateTimeProvider, tenantId, environmentId)
        {
            Command = inserirReclamacaoCommand
        };
        Apply(reclamacaoInserida);
        DomainEvents.Add(reclamacaoInserida);
    }

    private void Apply(ReclamacaoInserida reclamacao)
    {
        var reclamacaoAInserir = new ReclamacaoNaoConformidade();
        reclamacaoAInserir.Id = reclamacao.Command.ReclamacaoNaoConformidade.Id;
        reclamacaoAInserir.IdNaoConformidade = reclamacao.Command.ReclamacaoNaoConformidade.IdNaoConformidade;
        reclamacaoAInserir.Improcedentes = reclamacao.Command.ReclamacaoNaoConformidade.Improcedentes;
        reclamacaoAInserir.Observacao = reclamacao.Command.ReclamacaoNaoConformidade.Observacao;
        reclamacaoAInserir.Procedentes = reclamacao.Command.ReclamacaoNaoConformidade.Procedentes;
        reclamacaoAInserir.Recodificar = reclamacao.Command.ReclamacaoNaoConformidade.Recodificar;
        reclamacaoAInserir.Sucata = reclamacao.Command.ReclamacaoNaoConformidade.Sucata;
        reclamacaoAInserir.DevolucaoFornecedor = reclamacao.Command.ReclamacaoNaoConformidade.DevolucaoFornecedor;
        reclamacaoAInserir.QuantidadeLote = reclamacao.Command.ReclamacaoNaoConformidade.QuantidadeLote;
        reclamacaoAInserir.DisposicaoProdutosAprovados =
            reclamacao.Command.ReclamacaoNaoConformidade.DisposicaoProdutosAprovados;
        reclamacaoAInserir.DisposicaoProdutosConcessao =
            reclamacao.Command.ReclamacaoNaoConformidade.DisposicaoProdutosConcessao;
        reclamacaoAInserir.Rejeitado = reclamacao.Command.ReclamacaoNaoConformidade.Rejeitado;
        reclamacaoAInserir.Retrabalho = reclamacao.Command.ReclamacaoNaoConformidade.Retrabalho;
        reclamacaoAInserir.QuantidadeNaoConformidade =
            reclamacao.Command.ReclamacaoNaoConformidade.QuantidadeNaoConformidade;
        reclamacaoAInserir.RetrabalhoComOnus = reclamacao.Command.ReclamacaoNaoConformidade.RetrabalhoComOnus;
        reclamacaoAInserir.RetrabalhoSemOnus = reclamacao.Command.ReclamacaoNaoConformidade.RetrabalhoSemOnus;
        reclamacaoAInserir.CompanyId = reclamacao.Command.ReclamacaoNaoConformidade.CompanyId;
        ReclamacaoNaoConformidadeInserir = reclamacaoAInserir;
    }

    public void Process(AlterarReclamacaoNaoConformidadeCommand inserirReclamacaoCommand,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var reclamacaoAtualizada = new ReclamacaoAtualizada(dateTimeProvider, tenantId, environmentId)
        {
            Command = inserirReclamacaoCommand
        };
        Apply(reclamacaoAtualizada);
        DomainEvents.Add(reclamacaoAtualizada);
    }

    private void Apply(ReclamacaoAtualizada reclamacao)
    {
        var reclamacaoAAtualizar = ReclamacaoNaoConformidade;
        reclamacaoAAtualizar.Improcedentes = reclamacao.Command.ReclamacaoNaoConformidade.Improcedentes;
        reclamacaoAAtualizar.Observacao = reclamacao.Command.ReclamacaoNaoConformidade.Observacao;
        reclamacaoAAtualizar.Procedentes = reclamacao.Command.ReclamacaoNaoConformidade.Procedentes;
        reclamacaoAAtualizar.Recodificar = reclamacao.Command.ReclamacaoNaoConformidade.Recodificar;
        reclamacaoAAtualizar.Sucata = reclamacao.Command.ReclamacaoNaoConformidade.Sucata;
        reclamacaoAAtualizar.DevolucaoFornecedor = reclamacao.Command.ReclamacaoNaoConformidade.DevolucaoFornecedor;
        reclamacaoAAtualizar.QuantidadeLote = reclamacao.Command.ReclamacaoNaoConformidade.QuantidadeLote;
        reclamacaoAAtualizar.DisposicaoProdutosAprovados =
            reclamacao.Command.ReclamacaoNaoConformidade.DisposicaoProdutosAprovados;
        reclamacaoAAtualizar.DisposicaoProdutosConcessao =
            reclamacao.Command.ReclamacaoNaoConformidade.DisposicaoProdutosConcessao;
        reclamacaoAAtualizar.Rejeitado = reclamacao.Command.ReclamacaoNaoConformidade.Rejeitado;
        reclamacaoAAtualizar.Retrabalho = reclamacao.Command.ReclamacaoNaoConformidade.Retrabalho;
        reclamacaoAAtualizar.QuantidadeNaoConformidade =
            reclamacao.Command.ReclamacaoNaoConformidade.QuantidadeNaoConformidade;
        reclamacaoAAtualizar.RetrabalhoComOnus = reclamacao.Command.ReclamacaoNaoConformidade.RetrabalhoComOnus;
        reclamacaoAAtualizar.RetrabalhoSemOnus = reclamacao.Command.ReclamacaoNaoConformidade.RetrabalhoSemOnus;
        reclamacaoAAtualizar.CompanyId = reclamacao.Command.ReclamacaoNaoConformidade.CompanyId;
        ReclamacaoNaoConformidadeAlterar = reclamacaoAAtualizar;
    }

    //Implementação Evitar reincidencia
    public void Process(InserirImplementacaoEvitarReincidenciaNaoConformidadeCommand command,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var entidadeInserida =
            new ImplementacaoEvitarReincidenciaInserida(dateTimeProvider, tenantId, environmentId)
            {
                Command = command
            };
        Apply(entidadeInserida);
        DomainEvents.Add(entidadeInserida);
    }

    private void Apply(ImplementacaoEvitarReincidenciaInserida implementacao)
    {
        var implementacaoAInserir = new ImplementacaoEvitarReincidenciaNaoConformidade();
        implementacaoAInserir.Id = implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.Id;
        implementacaoAInserir.IdNaoConformidade =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdNaoConformidade;
        implementacaoAInserir.IdDefeitoNaoConformidade = implementacao.Command
            .ImplementacaoEvitarReincidenciaNaoConformidade.IdDefeitoNaoConformidade;
        implementacaoAInserir.Descricao =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.Descricao;
        implementacaoAInserir.DataAnalise =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.DataAnalise;
        implementacaoAInserir.DataVerificacao =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.DataVerificacao;
        implementacaoAInserir.IdAuditor =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdAuditor;
        implementacaoAInserir.AcaoImplementada =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.AcaoImplementada;
        implementacaoAInserir.IdResponsavel =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdResponsavel;
        implementacaoAInserir.NovaData = implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.NovaData;
        implementacaoAInserir.DataPrevistaImplantacao = implementacao.Command
            .ImplementacaoEvitarReincidenciaNaoConformidade.DataPrevistaImplantacao;
        implementacaoAInserir.CompanyId =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.CompanyId;
        ImplementacaoEvitarReincidenciaAInserir.Add(implementacaoAInserir);
        ImplementacaoEvitarReincidencia.Add(implementacaoAInserir);
    }

    public void Process(AlterarImplementacaoEvitarReincidenciaNaoConformidadeCommand command,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var acaoPreventivaAlterada =
            new ImplementacaoEvitarReincidenciaAtualizada(dateTimeProvider, tenantId, environmentId)
            {
                Command = command
            };
        Apply(acaoPreventivaAlterada);
        DomainEvents.Add(acaoPreventivaAlterada);
    }

    private void Apply(ImplementacaoEvitarReincidenciaAtualizada implementacao)
    {
        var implementacaoAAtualizar =
            ImplementacaoEvitarReincidencia.First(e =>
                e.Id == implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.Id);
        implementacaoAAtualizar.Descricao =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.Descricao;
        implementacaoAAtualizar.DataAnalise =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.DataAnalise;
        implementacaoAAtualizar.DataVerificacao =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.DataVerificacao;
        implementacaoAAtualizar.IdAuditor =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdAuditor;
        implementacaoAAtualizar.AcaoImplementada =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.AcaoImplementada;
        implementacaoAAtualizar.IdResponsavel =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdResponsavel;
        implementacaoAAtualizar.NovaData =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.NovaData;
        implementacaoAAtualizar.DataPrevistaImplantacao = implementacao.Command
            .ImplementacaoEvitarReincidenciaNaoConformidade.DataPrevistaImplantacao;
        implementacaoAAtualizar.CompanyId =
            implementacao.Command.ImplementacaoEvitarReincidenciaNaoConformidade.CompanyId;
        ImplementacaoEvitarReincidenciaAAlterar.Add(implementacaoAAtualizar);
    }

    public void Process(RemoverImplementacaoEvitarReincidenciaNaoConformidadeCommand command,
        IDateTimeProvider dateTimeProvider,
        Guid tenantId, Guid environmentId)
    {
        var entidadeRemovida =
            new ImplementacaoEvitarReincidenciaRemovida(dateTimeProvider, tenantId, environmentId)
            {
                Id = command.Id
            };
        Apply(entidadeRemovida);
        DomainEvents.Add(entidadeRemovida);
    }

    private void Apply(ImplementacaoEvitarReincidenciaRemovida implementacao)
    {
        var entidadeARemover =
            ImplementacaoEvitarReincidencia.First(e => e.Id.Equals(implementacao.Id));
        ImplementacaoEvitarReincidenciaARemover.Add(entidadeARemover);
        ImplementacaoEvitarReincidencia.Remove(entidadeARemover);
    }

    private void InserirCentrosCustoQueNaoEstaoNaAgregacao(CausaNaoConformidadeModel causaNaoConformidade,
        IDateTimeProvider dateTimeProvider, Guid tenantId, Guid environmentId, Guid companyId)
    {
        var idsCentrosCustosCausaNaoConformidade = CentroCustoCausaNaoConformidades
            .Where(e => e.IdCausaNaoConformidade == causaNaoConformidade.Id)
            .Select(e => e.IdCentroCusto)
            .ToList();

        foreach (var idCentroCusto in causaNaoConformidade.IdsCentrosCustos)
        {
            if (idsCentrosCustosCausaNaoConformidade.Contains(idCentroCusto))
            {
                continue;
            }

            var centroCustoCausaNaoConformidadeInserido =
                new CentroCustoCausaNaoConformidadeInserido(dateTimeProvider, tenantId, environmentId)
                {
                    Command = new InserirCentroCustoCausaNaoConformidadeCommand(
                        new CentroCustoCausaNaoConformidadeModel
                        {
                            IdNaoConformidade = NaoConformidade.Id,
                            IdCausaNaoConformidade = causaNaoConformidade.Id,
                            IdCentroCusto = idCentroCusto,
                            CompanyId = companyId
                        })
                };

            Apply(centroCustoCausaNaoConformidadeInserido);
            DomainEvents.Add(centroCustoCausaNaoConformidadeInserido);
        }
    }

    private void RemoverCentrosCustoQueNaoEstaoNoInput(CausaNaoConformidadeModel causaNaoConformidade,
        IDateTimeProvider dateTimeProvider, Guid tenantId, Guid environmentId)
    {
        var idCentroCustoPorIdCentroCustoCausaNaoConformidade = CentroCustoCausaNaoConformidades
            .Where(e => e.IdCausaNaoConformidade == causaNaoConformidade.Id)
            .ToDictionary(e => e.Id, e => e.IdCentroCusto);

        foreach (var (id, idCentroCusto) in idCentroCustoPorIdCentroCustoCausaNaoConformidade)
        {
            if (causaNaoConformidade.IdsCentrosCustos.Contains(idCentroCusto))
            {
                continue;
            }

            var centroCustoCausaNaoConformidadeRemovido =
                new CentroCustoCausaNaoConformidadeRemovido(dateTimeProvider, tenantId, environmentId)
                {
                    Id = id
                };

            Apply(centroCustoCausaNaoConformidadeRemovido);
            DomainEvents.Add(centroCustoCausaNaoConformidadeRemovido);
        }
    }
}
