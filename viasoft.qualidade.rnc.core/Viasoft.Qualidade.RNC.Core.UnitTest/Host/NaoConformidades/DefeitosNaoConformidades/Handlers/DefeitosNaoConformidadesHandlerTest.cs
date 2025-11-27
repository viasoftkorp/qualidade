using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.DefeitosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.DefeitosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Services;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Handlers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.DefeitosNaoConformidades.Handlers;

public class DefeitosNaoConformidadesHandlerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Handle DefeitoNaoConformidadeRemovido")]
    public async Task HandleDefeitoNaoConformidadeRemovido()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetService(mocker);
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var causa = TestUtils.ObjectMother.GetCausaNaoConformidade(0);
        var acao = TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidade(0);
        var solucao = TestUtils.ObjectMother.GetSolucaoNaoConformidade(0);
        await mocker.CausaNaoConformidades.InsertAsync(causa);
        await mocker.AcaoPreventivaNaoConformidades.InsertAsync(acao);
        await mocker.SolucaoNaoConformidades.InsertAsync(solucao);
        await UnitOfWork.SaveChangesAsync();

        //Act
        await handler.Handle(new DefeitoNaoConformidadeRemovido { IdDefeito = TestUtils.ObjectMother.Guids[0] });

        //Assert
        var causaOutput = await mocker.CausaNaoConformidadeService.Get(idNaoConformidade, causa.Id);
        causaOutput.Should().BeNull();
        var solucaoOutput = await mocker.SolucaoNaoConformidadeService.Get(idNaoConformidade, solucao.Id);
        solucaoOutput.Should().BeNull();
        var acaoOutput = await mocker.AcaoPreventivaNaoConformidadeService.Get(idNaoConformidade, acao.Id);
        acaoOutput.Should().BeNull();
    }

     [Fact(DisplayName = "Handle DefeitoNaoConformidadeInserido")]
     public async Task HandleDefeitoNaoConformidadeInserido()
     {
         //Arrange
         var mocker = GetMocker();
         var handler = GetService(mocker);
        
         var defeitoNaoConformidade = new DefeitoNaoConformidadeInput(TestUtils.ObjectMother.GetDefeitoNaoConformidade(0));

         var defeito = new DefeitoOutput(TestUtils.ObjectMother.GetDefeito(0));
         var causaOutput = new CausaOutput(TestUtils.ObjectMother.GetCausa(0));
         var solucao = new SolucaoOutput(TestUtils.ObjectMother.GetSolucao(0));
    
         mocker.DefeitoService.Get(defeitoNaoConformidade.IdDefeito).Returns(defeito);
         mocker.CausaService.Get(defeito.IdCausa.Value).Returns(causaOutput);
         mocker.SolucaoService.Get(defeito.IdSolucao.Value).Returns(solucao);
         var evento = new DefeitoNaoConformidadeInserido { Command = new InserirDefeitoCommand(defeitoNaoConformidade) };
    
         //Act
         await handler.Handle(evento);
    
         //Assert
         handler.Handle(evento).IsCompletedSuccessfully.Should().BeTrue();
     }

     [Fact(DisplayName = "Handle DefeitoNaoConformidadeAtualizado")]
     public async Task HandleDefeitoNaoConformidadeAtualizado()
     {
         //Arrange
         var mocker = GetMocker();
         var handler = GetService(mocker);

         var defeitoNaoConformidade =
             new DefeitoNaoConformidadeInput(TestUtils.ObjectMother.GetDefeitoNaoConformidade(0));

         var defeito = new DefeitoOutput(TestUtils.ObjectMother.GetDefeito(0));
         var causaOutput = new CausaOutput(TestUtils.ObjectMother.GetCausa(0));
         var solucao = new SolucaoOutput(TestUtils.ObjectMother.GetSolucao(0));

         mocker.DefeitoService.Get(defeitoNaoConformidade.IdDefeito).Returns(defeito);
         mocker.CausaService.Get(defeito.IdCausa.Value).Returns(causaOutput);
         mocker.SolucaoService.Get(defeito.IdSolucao.Value).Returns(solucao);
         var evento = new DefeitoNaoConformidadeAtualizado
             { Command = new AlterarDefeitoCommand(defeitoNaoConformidade) };

         //Act
         await handler.Handle(evento);

         //Assert
         handler.Handle(evento).IsCompletedSuccessfully.Should().BeTrue();
     }

     private DefeitoNaoConformidadeHandlerMocker GetMocker()
    {
        var mocker = new DefeitoNaoConformidadeHandlerMocker
        {
            AcaoPreventivaNaoConformidadeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>(),
            AcaoPreventivaNaoConformidades = ServiceProvider.GetService<IRepository<AcaoPreventivaNaoConformidade>>(),
            CausaNaoConformidadeService = Substitute.For<ICausaNaoConformidadeService>(),
            CausaNaoConformidades = ServiceProvider.GetService<IRepository<CausaNaoConformidade>>(),
            SolucaoNaoConformidadeService = Substitute.For<ISolucaoNaoConformidadeService>(),
            SolucaoNaoConformidades = ServiceProvider.GetService<IRepository<SolucaoNaoConformidade>>(),
            DefeitoService = Substitute.For<IDefeitoService>(),
            CausaService = Substitute.For<ICausaService>(),
            SolucaoService = Substitute.For<ISolucaoService>(),
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>()
        };
        return mocker;
    }

    private DefeitosNaoConformidadesHandler GetService(DefeitoNaoConformidadeHandlerMocker mocker)
    {
        var handler = new DefeitosNaoConformidadesHandler(mocker.AcaoPreventivaNaoConformidadeService,
            mocker.AcaoPreventivaNaoConformidades, mocker.CausaNaoConformidadeService,
            mocker.CausaNaoConformidades, mocker.SolucaoNaoConformidadeService, mocker.SolucaoNaoConformidades,
            mocker.DefeitoService, mocker.CausaService, mocker.SolucaoService);

        return handler;
    }

    public class DefeitoNaoConformidadeHandlerMocker
    {
        public IAcaoPreventivaNaoConformidadeService AcaoPreventivaNaoConformidadeService { get; set; }
        public IRepository<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidades { get; set; }
        public ICausaNaoConformidadeService CausaNaoConformidadeService { get; set; }
        public IRepository<CausaNaoConformidade> CausaNaoConformidades { get; set; }
        public ISolucaoNaoConformidadeService SolucaoNaoConformidadeService { get; set; }
        public IRepository<SolucaoNaoConformidade> SolucaoNaoConformidades { get; set; }
        public IDefeitoService DefeitoService { get; set; }
        public ICausaService CausaService { get; set; }
        public ISolucaoService SolucaoService { get; set; }
        public ICausaNaoConformidadeViewService CausaNaoConformidadeViewService { get; set; }
        public ISolucaoNaoConformidadeViewService SolucaoNaoConformidadeViewService { get; set; }
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
    }
}