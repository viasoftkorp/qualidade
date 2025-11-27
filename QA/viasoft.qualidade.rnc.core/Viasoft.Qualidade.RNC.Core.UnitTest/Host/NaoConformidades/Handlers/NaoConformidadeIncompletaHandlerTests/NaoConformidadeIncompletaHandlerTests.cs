using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Handlers;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Handlers.NaoConformidadeIncompletaHandlerTests;

public class NaoConformidadeIncompletaHandlerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se for a não conformidade for encontrada e ela estiver incompleta, deve deleta-la")]
    public async Task VerificarNaoConformidadeIncompletaMessageHandleTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new VerificarNaoConformidadeIncompletaMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0]
        };
        var naoConformidade = TestUtils.ObjectMother.GetNaoConformidade(0);
        naoConformidade.Incompleta = true;
        await mocker.NaoConformidades.InsertAsync(naoConformidade, true);
        
        //Act
        await handler.Handle(message);
        
        //Assert
        mocker.NaoConformidades.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Se nao conformidade for encontrada mas não estiver incompleta, nada deve acontecer")]
    public async Task VerificarNaoConformidadeIncompletaMessageHandleTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new VerificarNaoConformidadeIncompletaMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0]
        };
        var naoConformidade = TestUtils.ObjectMother.GetNaoConformidade(0);
        naoConformidade.Incompleta = false;
        await mocker.NaoConformidades.InsertAsync(naoConformidade, true);
        
        //Act
        await handler.Handle(message);
        
        //Assert
        mocker.NaoConformidades.Should().NotBeEmpty();
    }
    
    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            NaoConformidades = ServiceProvider.GetService<IRepository<NaoConformidade>>()
        };
        return mocker;
    }

    protected NaoConformidadeIncompletaHandler GetHandler(Mocker mocker)
    {
        var service = new NaoConformidadeIncompletaHandler(mocker.NaoConformidades);

        return service;
    }

    public class Mocker
    {
        public IRepository<NaoConformidade> NaoConformidades { get; set; }
    }
}