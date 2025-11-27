using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNaoConformidadesFechadasSemConclusaoSeeders;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Seeders.CorrigirNaoConformidadesFechadasSemConclusaoSeeders;

public class CorrigirNaoConformidadesFechadasSemConclusaoHandlerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Para cada não conformidade fechada sem conclusão, deve alterar seu status para \"aberto\"")]
    public async Task CorrigirNaoConformidadesFechadasSemConclusaoMessageHandleTest()
    {
        // Arrange
        var mocker = GetMocker();
        var handler = GetSeederHandler(mocker);
        
        var naoConformidades = new List<NaoConformidade>
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3),
            TestUtils.ObjectMother.GetNaoConformidade(4),
        };
        naoConformidades[0].Status = StatusNaoConformidade.Fechado;
        naoConformidades[1].Status = StatusNaoConformidade.Fechado;
        naoConformidades[2].Status = StatusNaoConformidade.Fechado;

        naoConformidades[3].Status = StatusNaoConformidade.Pendente;

        naoConformidades[4].Status = StatusNaoConformidade.Aberto;
        
        await mocker.NaoConformidades.InsertRangeAsync(naoConformidades, true);
        await mocker.ConclusaoNaoConformidades.InsertAsync(TestUtils.ObjectMother.GetConclusaoNaoConformidade(2), true);
        await mocker.SeederManagers.InsertAsync(new SeederManager(), true);
        var message = new CorrigirNaoConformidadesFechadasSemConclusaoMessage();

        var expectedResult = new List<NaoConformidade>
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3),
            TestUtils.ObjectMother.GetNaoConformidade(4),
        };
        
        expectedResult[0].Status = StatusNaoConformidade.Aberto;
        expectedResult[1].Status = StatusNaoConformidade.Aberto;
        expectedResult[2].Status = StatusNaoConformidade.Fechado;

        expectedResult[3].Status = StatusNaoConformidade.Pendente;

        expectedResult[4].Status = StatusNaoConformidade.Aberto;
        
        // Act
        await handler.Handle(message);
        TestUtils.LimparTracker(mocker.NaoConformidades);
        // Assert
        var naoConformidadesResult = await mocker.NaoConformidades.ToListAsync();
        naoConformidadesResult.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }
    
    [Fact(DisplayName = "Se sucesso ao corrigir nao conformidades fechadas sem conclusao, " +
                        "deve alterar CorrigirNaoConformidadesFechadasSemConclusaoSeederFinalizado para true")]
    public async Task CorrigirNaoConformidadesFechadasSemConclusaoMessageHandleTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var handler = GetSeederHandler(mocker);

        var message = new CorrigirNaoConformidadesFechadasSemConclusaoMessage();

        var naoConformidades = new List<NaoConformidade>
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3),
            TestUtils.ObjectMother.GetNaoConformidade(4),
        };
        naoConformidades[0].Status = StatusNaoConformidade.Fechado;
        naoConformidades[1].Status = StatusNaoConformidade.Fechado;
        naoConformidades[2].Status = StatusNaoConformidade.Fechado;

        naoConformidades[3].Status = StatusNaoConformidade.Pendente;

        naoConformidades[4].Status = StatusNaoConformidade.Aberto;
        
        await mocker.NaoConformidades.InsertRangeAsync(naoConformidades, true);
        await mocker.ConclusaoNaoConformidades.InsertAsync(TestUtils.ObjectMother.GetConclusaoNaoConformidade(2), true);
        await mocker.SeederManagers.InsertAsync(new SeederManager(), true);
        
        var expectedResult = new List<NaoConformidade>
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3),
            TestUtils.ObjectMother.GetNaoConformidade(4),
        };
        
        naoConformidades[0].Status = StatusNaoConformidade.Aberto;
        naoConformidades[1].Status = StatusNaoConformidade.Aberto;
        naoConformidades[2].Status = StatusNaoConformidade.Fechado;

        naoConformidades[3].Status = StatusNaoConformidade.Pendente;

        naoConformidades[4].Status = StatusNaoConformidade.Aberto;
        
        // Act
        await handler.Handle(message);
        TestUtils.LimparTracker(mocker.NaoConformidades);
        // Assert
        var seederManager = await mocker.SeederManagers.FirstAsync();
        seederManager.CorrigirNaoConformidadesFechadasSemConclusaoSeederFinalizado.Should().BeTrue();
    }

    private class Mocker
    {
        public IRepository<SeederManager> SeederManagers { get; set; }
        public IRepository<NaoConformidade> NaoConformidades { get; set; }
        public IRepository<ConclusaoNaoConformidade> ConclusaoNaoConformidades { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            SeederManagers = ServiceProvider.GetService<IRepository<SeederManager>>(),
            NaoConformidades = ServiceProvider.GetService<IRepository<NaoConformidade>>(),
            ConclusaoNaoConformidades = ServiceProvider.GetService<IRepository<ConclusaoNaoConformidade>>(),
        };

        return mocker;
    }

    private CorrigirNaoConformidadesFechadasSemConclusaoHandler GetSeederHandler(Mocker mocker)
    {
        var service = new CorrigirNaoConformidadesFechadasSemConclusaoHandler(mocker.SeederManagers,
            mocker.NaoConformidades, mocker.ConclusaoNaoConformidades, UnitOfWork);
        return service;
    }
}