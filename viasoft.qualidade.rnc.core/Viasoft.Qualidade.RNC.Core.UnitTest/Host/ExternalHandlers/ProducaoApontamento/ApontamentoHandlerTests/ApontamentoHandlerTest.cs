using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.ProducaoApontamento.Operacoes;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public abstract class ApontamentoHandlerTest : TestUtils.UnitTestBaseWithDbContext
{
    protected void LimparTracker(Mocker mocker)
    {
        mocker.Operacoes.GetUnderlyingDbContext().ChangeTracker.Clear();
    }
    protected async Task InserirNaoConformidade(int index)
    {
        var naoConformidadesRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>();

        var naoConformidade = TestUtils.ObjectMother.GetNaoConformidade(0);
        await naoConformidadesRepository.InsertAsync(naoConformidade);   
    }

    protected async Task InserirOperacaoRetrabalhoNaoConformidade(OperacaoRetrabalhoNaoConformidade expectedOperacaoRetrabalhoNaoConformidade)
    {
        var operacaoRetrabalhoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<OperacaoRetrabalhoNaoConformidade>>();
        
        await operacaoRetrabalhoNaoConformidadeRepository.InsertAsync(expectedOperacaoRetrabalhoNaoConformidade, true);
    }
    
    protected OrdemRetrabalhoNaoConformidade GetOrdemRetrabalhoNaoConformidade(int index)
    {
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[index],
            CreationTime = TestUtils.ObjectMother.Datas[index],
            CreatorId = TestUtils.ObjectMother.Guids[index],
            LastModificationTime = TestUtils.ObjectMother.Datas[index],
            LastModifierId = TestUtils.ObjectMother.Guids[index],
            DeleterId = null,
            DeletionTime = null,
            IsDeleted = false,
            EnvironmentId = TestUtils.ObjectMother.Guids[index],
            TenantId = TestUtils.ObjectMother.Guids[index],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[index],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[index],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[index],
            IdLocalDestino = TestUtils.ObjectMother.Guids[index],
            MovimentacaoEstoqueMensagemRetorno = TestUtils.ObjectMother.Strings[index],
            CodigoArmazem = TestUtils.ObjectMother.Ints[index].ToString(),
            DataFabricacao = TestUtils.ObjectMother.Datas[index],
            DataValidade = TestUtils.ObjectMother.Datas[index],
            Status = StatusProducaoRetrabalho.Aberta
        };
        return ordemRetrabalhoNaoConformidade;
    }

    protected OperacaoRetrabalhoNaoConformidade GetOperacaoRetrabalhoNaoConformidade(int index)
    {
        var operacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[index],
            CreationTime = TestUtils.ObjectMother.Datas[index],
            CreatorId = TestUtils.ObjectMother.Guids[index],
            LastModificationTime = TestUtils.ObjectMother.Datas[index],
            LastModifierId = TestUtils.ObjectMother.Guids[index],
            DeleterId = null,
            DeletionTime = null,
            IsDeleted = false,
            IdNaoConformidade = TestUtils.ObjectMother.Guids[index],
            EnvironmentId = TestUtils.ObjectMother.Guids[index],
            TenantId = TestUtils.ObjectMother.Guids[index],
            Quantidade = 0,
            NumeroOperacaoARetrabalhar = TestUtils.ObjectMother.Ints[index].ToString(),
        };
        return operacaoRetrabalhoNaoConformidade;
    }
    protected Operacao GetOperacao(int index)
    {
        var operacao = new Operacao
        {
            Id = TestUtils.ObjectMother.Guids[index],
            CreationTime = TestUtils.ObjectMother.Datas[index],
            CreatorId = TestUtils.ObjectMother.Guids[index],
            LastModificationTime = TestUtils.ObjectMother.Datas[index],
            LastModifierId = TestUtils.ObjectMother.Guids[index],
            DeleterId = null,
            DeletionTime = null,
            IsDeleted = false,
            EnvironmentId = TestUtils.ObjectMother.Guids[index],
            TenantId = TestUtils.ObjectMother.Guids[index],
            Status = StatusProducaoRetrabalho.Aberta,
            IdRecurso = TestUtils.ObjectMother.Guids[index],
            NumeroOperacao = TestUtils.ObjectMother.Ints[index]
                .ToString(),
            IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[index],
            OperacaoRetrabalhoNaoConformidade = null

        };
        return operacao;
    }
    protected class Mocker
    {
        public IRepository<Operacao> Operacoes;
        public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidades { get; set; }
        public IPushNotification PushNotification { get; set; }
        public IOperacaoService OperacaoService { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            Operacoes = ServiceProvider.GetService<IRepository<Operacao>>(),
            OrdemRetrabalhoNaoConformidades = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>(),
            PushNotification = Substitute.For<IPushNotification>(),
            OperacaoService = Substitute.For<IOperacaoService>()
        };
        return mocker;
    }

    protected ApontamentoHandler GetHandler(Mocker mocker)
    {
        var handler = new ApontamentoHandler(mocker.Operacoes, mocker.OrdemRetrabalhoNaoConformidades,
            mocker.PushNotification, mocker.OperacaoService);
        return handler;
    }
}