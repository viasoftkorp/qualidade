using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherLocaisSeeders;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Seeders.PreencherLocaisSeedersTests;

public class PreencherLocaisHandlerTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName =
        "Se houver ordens retrabalho, deve buscar os idsLocais de destino e origem e inserir aqueles que não estão inseridos")]
    public async Task SeedPreencherLocaisMessageHandleTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var handler = GetHandler(dependencies);
        var ordensRetrabalho = new List<OrdemRetrabalhoNaoConformidade>
        {
            TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(0),
            TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(1),
            TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(2),
        };
        ordensRetrabalho[0].IdLocalOrigem = TestUtils.ObjectMother.Guids[0];
        ordensRetrabalho[1].IdLocalOrigem = TestUtils.ObjectMother.Guids[1];
        ordensRetrabalho[2].IdLocalOrigem = TestUtils.ObjectMother.Guids[2];
        
        ordensRetrabalho[0].IdLocalDestino = TestUtils.ObjectMother.Guids[3];
        ordensRetrabalho[1].IdLocalDestino = TestUtils.ObjectMother.Guids[4];
        ordensRetrabalho[2].IdLocalDestino = TestUtils.ObjectMother.Guids[5];
        
        await dependencies.OrdemRetrabalhoNaoConformidades.InsertRangeAsync(ordensRetrabalho, true);
        await dependencies.SeederManagers.InsertAsync(new SeederManager(), true);

        var expectedIdsLocais = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3],
            TestUtils.ObjectMother.Guids[4],
            TestUtils.ObjectMother.Guids[5],
        };
        // Act
        await handler.Handle(new SeedPreencherLocaisMessage());
        
        // Assert
        await dependencies.LocalService
            .Received(1)
            .BatchInserirNaoCadastrados(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(expectedIdsLocais)));
    }
    
    [Fact(DisplayName = "Se tudo ocorrer bem, deve alterar PreencherLocaisSeederFinalizado para true")]
    public async Task SeedPreencherLocaisMessageHandleTest2()
    {
        // Arrange
        var dependencies = GetDependencies();
        var handler = GetHandler(dependencies);
        var ordensRetrabalho = new List<OrdemRetrabalhoNaoConformidade>
        {
            TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(0),
            TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(1),
            TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(2),
        };

        await dependencies.SeederManagers.InsertAsync(new SeederManager(), true);
        
        await dependencies.OrdemRetrabalhoNaoConformidades.InsertRangeAsync(ordensRetrabalho, true);
        
        // Act
        await handler.Handle(new SeedPreencherLocaisMessage());
        TestUtils.LimparTracker(dependencies.SeederManagers);
        
        // Assert
        var seederManagerResult = await dependencies.SeederManagers.FirstAsync();
        seederManagerResult.PreencherLocaisSeederFinalizado.Should().BeTrue();

    }
    private class Dependencies
    {
        public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidades { get; set; }
        public IRepository<SeederManager> SeederManagers { get; set; }
        public ILocalService LocalService { get; set; }
    }

    private Dependencies GetDependencies()
    {
        var dependencies = new Dependencies
        {
            LocalService = Substitute.For<ILocalService>(),
            SeederManagers = ServiceProvider.GetService<IRepository<SeederManager>>(),
            OrdemRetrabalhoNaoConformidades = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>()
        };
        return dependencies;
    }

    private PreencherLocaisHandler GetHandler(Dependencies dependencies)
    {
        var handler = new PreencherLocaisHandler(dependencies.OrdemRetrabalhoNaoConformidades, dependencies.SeederManagers,
            dependencies.LocalService);
        return handler;
    }
}