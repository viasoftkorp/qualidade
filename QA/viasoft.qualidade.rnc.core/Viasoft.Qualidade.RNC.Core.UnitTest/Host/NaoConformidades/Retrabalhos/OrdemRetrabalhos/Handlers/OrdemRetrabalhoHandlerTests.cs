
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Handlers;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Handlers;

public class OrdemRetrabalhoHandlerTests
{
    [Fact(DisplayName = "Se ordem retrabalho inserida, deve inserir local origem e local destino")]
    public async Task OrdemRetrabalhoNaoConformidadeInseridaHandleTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var handler = GetHandler(dependencies);

        var ordemRetrabalho = TestUtils.ObjectMother.GetOrdemRetrabalhoNaoConfrmidade(0);
        
        ordemRetrabalho.IdLocalDestino = TestUtils.ObjectMother.Guids[1];
        ordemRetrabalho.IdLocalDestino = TestUtils.ObjectMother.Guids[0];
        
        var message = new OrdemRetrabalhoNaoConformidadeInserida
        {
            OrdemRetrabalhoNaoConformidade = ordemRetrabalho
        };

        var expectedIdsToInsert = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
        };
        
        // Act
        await handler.Handle(message);
        // Assert
        await dependencies.LocalService
            .Received(1)
            .BatchInserirNaoCadastrados(Arg.Is<List<Guid>>(e => e.IsEquivalentTo(expectedIdsToInsert)));
    }
    
    protected class Dependencies
    {
        public ILocalService LocalService { get; set; }
    }

    protected Dependencies GetDependencies()
    {
        var mocker = new Dependencies
        {
           LocalService = Substitute.For<ILocalService>()
        };

        return mocker;
    }

    protected OrdemRetrabalhoHandler GetHandler(Dependencies dependencies)
    {
        var service = new OrdemRetrabalhoHandler(dependencies.LocalService);
        return service;
    }
}