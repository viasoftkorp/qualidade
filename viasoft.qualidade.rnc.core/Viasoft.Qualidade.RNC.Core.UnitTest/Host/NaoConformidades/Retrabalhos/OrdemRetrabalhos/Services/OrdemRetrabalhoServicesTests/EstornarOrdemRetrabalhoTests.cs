using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.OrdemRetrabalhoServicesTests;

public class EstornarOrdemRetrabalhoTests : OrdemRetrabalhoServiceTest
{
    [Fact(DisplayName = "Se falhar ao estornar odf de retrabalho, deve retornar o dto resultante")]
    public async Task GerarOrdemRetrabalhoTest1()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        MockGetAgregacaoReturn(mocker, agregacao);

        var ordemRetrabalhoNaoConformidade = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalhoNaoConformidade.MovimentacaoEstoqueMensagemRetorno = "";
        
        await mocker.Repository.InsertAsync(ordemRetrabalhoNaoConformidade, true);

        var externalInput = MockarGetExternalEstornarOrdemRetrabalhoInput(mocker, ordemRetrabalhoNaoConformidade, 0);
        
        MockarExternalEstornarOrdemRetrabalho(mocker, expectedInput: externalInput, expectedNaoConformidadeOutput: new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "Deu erro"
        });
        
        var expectedResult = new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "Deu erro"
        };
        //Act
        var result = await service.EstornarOrdemRetrabalho(agregacao, ordemRetrabalhoNaoConformidade, true);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se sucesso ao estornar odf de retrabalho e é pra notificar usuário, deve notificar usuário")]
    public async Task GerarOrdemRetrabalhoTest2()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.CreatorId = TestUtils.ObjectMother.Guids[0];
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        MockGetAgregacaoReturn(mocker, agregacao);
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = 1,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.Repository.InsertAsync(ordemRetrabalhoNaoConformidade, true);

        var externalInput = MockarGetExternalEstornarOrdemRetrabalhoInput(mocker, ordemRetrabalhoNaoConformidade, 0);
        
        MockarExternalEstornarOrdemRetrabalho(mocker, expectedInput: externalInput, expectedNaoConformidadeOutput: new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = true,
        });
        //Act
        var result = await service.EstornarOrdemRetrabalho(agregacao, ordemRetrabalhoNaoConformidade, true);

        //Assert
        await mocker.PushNotification.Received(1).SendAsync(Arg.Is<Payload>(e =>
                e.Header == "Odf retrabalho estornada com sucesso" 
                && e.Body == $"Número odf retrabalho: 1"),
            TestUtils.ObjectMother.Guids[0], true);    
    }

    private OrdemRetrabalhoNaoConformidade GetOrdemRetrabalhoNaoConformidade(int index)
    {
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[index],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[index],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[index],
            MovimentacaoEstoqueMensagemRetorno = TestUtils.ObjectMother.Strings[index]
        };
        return ordemRetrabalhoNaoConformidade;
    }

    private ExternalEstornarOrdemRetrabalhoInput MockarGetExternalEstornarOrdemRetrabalhoInput(Mocker mocker, OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade, int index)
    {
        var externalInput = new ExternalEstornarOrdemRetrabalhoInput
        {
            Odf = TestUtils.ObjectMother.Ints[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            CodigoProduto = TestUtils.ObjectMother.Strings[index],
            PedidoVenda = TestUtils.ObjectMother.Strings[index],
            OdfVenda = TestUtils.ObjectMother.Ints[index],
            SaldoOdf = TestUtils.ObjectMother.Ints[index],
            Situacao = TestUtils.ObjectMother.Strings[index],
            Motivo = TestUtils.ObjectMother.Strings[index]
        };
        
        mocker.OrdemRetrabalhoAclService
            .GetExternalEstornarOrdemRetrabalhoInput(TestUtils.ObjectMother.Ints[0], ordemRetrabalhoNaoConformidade)
            .Returns(externalInput);
        return externalInput;
    }
    private void MockarExternalEstornarOrdemRetrabalho(Mocker mocker, ExternalEstornarOrdemRetrabalhoInput expectedInput, 
        OrdemRetrabalhoNaoConformidadeOutput expectedNaoConformidadeOutput)
    {
        mocker.ExternalOrdemRetrabalhoService.EstornarOrdemRetrabalho(expectedInput).Returns(expectedNaoConformidadeOutput);
    }
}