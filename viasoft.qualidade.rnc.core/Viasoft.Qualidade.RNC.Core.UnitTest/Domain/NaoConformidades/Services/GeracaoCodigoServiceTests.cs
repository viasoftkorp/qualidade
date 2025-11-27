using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Domain.NaoConformidades.Services;

public class GeracaoCodigoServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName =
        "Se houver não conformidades, codigo deve ser o código da ultima não conformidade criada somado com 1")]
    public async Task GetCodigoNaoConformidadeTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidades = new List<NaoConformidade>
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3),
        };
        await mocker.NaoConformidades.InsertRangeAsync(naoConformidades, true);
        
        //Act
        var result = await service.GetCodigoNaoConformidade();
        
        //Assert
        result.Should().Be(5);
    }
    
    [Fact(DisplayName =
        "Se não houver não conformidades, codigo deve 1")]
    public async Task GetCodigoNaoConformidadeTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        //Act
        var result = await service.GetCodigoNaoConformidade();
        
        //Assert
        result.Should().Be(1);
    }
    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            NaoConformidades = ServiceProvider.GetService<IRepository<NaoConformidade>>()
        };
        return mocker;
    }

    protected GeracaoCodigoService GetService(Mocker mocker)
    {
        var service = new GeracaoCodigoService(mocker.NaoConformidades);

        return service;
    }

    protected class Mocker
    {
        public IRepository<NaoConformidade> NaoConformidades { get; set; }
    }
}