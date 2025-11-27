using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherDataCriacaoNaoConformidadesSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherDataCriacaoNaoConformidadesSeeders.Contracts;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Seeders.PreencherDataCriacaoNaoConformidadeSeederTests;

public class PreencherDataCriacaoNaoConformidadeHandlerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName =
        "Se método chamado, deve alterar \"dataCriacao\" de todas as não conformidades para o valor em creationTime")]
    public async Task SeedPreencherDataCriacaoNaoConformidadesMessageHandleTest()
    {
        //Arrange
        var dependencies = GetDependencies();
        var handler = GetHandler(dependencies);
        var naoConformidades = new List<NaoConformidade>()
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3)
        };
        naoConformidades[0].CreationTime = TestUtils.ObjectMother.Datas[0];
        naoConformidades[1].CreationTime = TestUtils.ObjectMother.Datas[1];
        naoConformidades[2].CreationTime = TestUtils.ObjectMother.Datas[2];
        naoConformidades[3].CreationTime = TestUtils.ObjectMother.Datas[3];
        
        await dependencies.NaoConformidades.InsertRangeAsync(naoConformidades, true);
        await dependencies.SeederManagers.InsertAsync(new SeederManager(), true);
        var message = new SeedPreencherDataCriacaoNaoConformidadesMessage();

        var expectedResult = naoConformidades;

        expectedResult[0].DataCriacao = TestUtils.ObjectMother.Datas[0];
        expectedResult[1].DataCriacao = TestUtils.ObjectMother.Datas[1];
        expectedResult[2].DataCriacao = TestUtils.ObjectMother.Datas[2];
        expectedResult[3].DataCriacao = TestUtils.ObjectMother.Datas[3];
        //Act
        await handler.Handle(message);
        TestUtils.LimparTracker(dependencies.NaoConformidades);
        //Assert
        var naoConformidadesResult = await dependencies.NaoConformidades.ToListAsync();
        naoConformidadesResult.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName =
        "Se método chamado, deve alterar setar para \"PreencherDataCriacaoNaoConformidadeSeederFinalizado\" true")]
    public async Task SeedPreencherDataCriacaoNaoConformidadesMessageHandleTest2()
    {
        //Arrange
        var dependencies = GetDependencies();
        var handler = GetHandler(dependencies);
        
        await dependencies.SeederManagers.InsertAsync(new SeederManager(), true);
        var message = new SeedPreencherDataCriacaoNaoConformidadesMessage();
        //Act
        await handler.Handle(message);
        TestUtils.LimparTracker(dependencies.NaoConformidades);
        //Assert
        var seederManagerResult = await dependencies.SeederManagers.FirstAsync();
        seederManagerResult.PreencherDataCriacaoNaoConformidadeSeederFinalizado.Should().BeTrue();
    }
    public class Dependencies
    {
        public IRepository<NaoConformidade> NaoConformidades {get;set;}
        public IRepository<SeederManager> SeederManagers {get;set;}
    }

    public Dependencies GetDependencies()
    {
        var dependencies = new Dependencies
        {
            NaoConformidades = ServiceProvider.GetService<IRepository<NaoConformidade>>(),
            SeederManagers = ServiceProvider.GetService<IRepository<SeederManager>>()
        };
        return dependencies;
    }

    public PreencherDataCriacaoNaoConformidadeHandler GetHandler(Dependencies dependencies)
    {
        var handler = new PreencherDataCriacaoNaoConformidadeHandler(dependencies.NaoConformidades, UnitOfWork, 
            dependencies.SeederManagers);
        return handler;
    }
}