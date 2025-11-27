using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    OrdemRetrabalhoServicesTests;

public class GerarOrdemRetrabalhoTests : OrdemRetrabalhoServiceTest
{
    [Fact(DisplayName = "Se falhar ao gerar odf de retrabalho, deve retornar o dto resultante")]
    public async Task GerarOrdemRetrabalhoTest1()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0]
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        agregacao.NaoConformidade.NumeroLote = TestUtils.ObjectMother.Strings[0];
        MockValidatorServiceReturn(mocker, agregacao);
        
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        });

        var getGerarOrdemRetrabalhoInput = GetGerarOrdemRetrabalhoInput(0);
        
        var entidadeMockada = MockarGetExternalGerarOrdemRetrabalhoInput(mocker, getGerarOrdemRetrabalhoInput, 0, ignoreList: new List<string>
        {
            nameof(GerarOrdemRetrabalhoInput.MateriaisInput),
            nameof(GerarOrdemRetrabalhoInput.MaquinasInput)
        });

        MockarExternalGerarOrdemRetrabalho(mocker, expectedInput: entidadeMockada,
            expectedOutput: new ExternalGerarOrdemRetrabalhoOutput
            {
                Success = false,
                OdfGerada = 901
            });
        
        var expectedResult = new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            NumeroOdfRetrabalho = 901
        };
        
        //Act
        var result = await service.GerarOrdemRetrabalho(agregacao, input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se sucesso ao gerar odf de retrabalho, deve salvar odf de retrabalho não conformidade")]
    public async Task GerarOrdemRetrabalhoTest2()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        agregacao.NaoConformidade.NumeroLote = TestUtils.ObjectMother.Strings[0];
        MockValidatorServiceReturn(mocker, agregacao);
        
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        });

        var getGerarOrdemRetrabalhoInput = GetGerarOrdemRetrabalhoInput(0);
        
        var entidadeMockada = MockarGetExternalGerarOrdemRetrabalhoInput(mocker, getGerarOrdemRetrabalhoInput, 0, ignoreList: new List<string>
        {
            nameof(GerarOrdemRetrabalhoInput.MateriaisInput),
            nameof(GerarOrdemRetrabalhoInput.MaquinasInput)
        });

        MockarExternalGerarOrdemRetrabalho(mocker, expectedInput: entidadeMockada,
            expectedOutput: new ExternalGerarOrdemRetrabalhoOutput
            {
                Success = true,
                OdfGerada = 901
            });
        
        mocker.LocalProvider.GetByCode(TestUtils.ObjectMother.Ints[0]).Returns(new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        });
        mocker.EstoqueLocalAclService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new EstoqueLocalOutput
        {
            CodigoLocal = TestUtils.ObjectMother.Ints[0],
            CodigoArmazem = TestUtils.ObjectMother.Ints[0].ToString(),
            DataFabricacao = TestUtils.ObjectMother.Datas[0],
            DataValidade = TestUtils.ObjectMother.Datas[0]
        });
        var expectedResult = new OrdemRetrabalhoNaoConformidade
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = 901,
            Quantidade = 1,
            MovimentacaoEstoqueMensagemRetorno = null,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            CodigoArmazem = TestUtils.ObjectMother.Ints[0].ToString(),
            DataFabricacao = TestUtils.ObjectMother.Datas[0],
            DataValidade = TestUtils.ObjectMother.Datas[0]
        };
        //Act
        var result = await service.GerarOrdemRetrabalho(agregacao, input);

        //Assert
        var odfRetrabalhoNaoConformidadeCriada = await mocker.Repository.FirstAsync(e =>
            e.NumeroOdfRetrabalho == 901 &&
            e.IdNaoConformidade == TestUtils.ObjectMother.Guids[0]);
        odfRetrabalhoNaoConformidadeCriada.Should().BeEquivalentTo(expectedResult, options =>
            TestUtils.ExcludeAuditoria(options)
                .Excluding(e => e.Id));
    }

    [Fact(DisplayName = "Se sucesso ao gerar odf de retrabalho, deve notificar o usuário")]
    public async Task GerarOrdemRetrabalhoTest3()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0],
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        agregacao.NaoConformidade.NumeroLote = TestUtils.ObjectMother.Strings[0];
        MockValidatorServiceReturn(mocker, agregacao);
        
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        });
        
        var getGerarOrdemRetrabalhoInput = GetGerarOrdemRetrabalhoInput(0);
        
        var entidadeMockada = MockarGetExternalGerarOrdemRetrabalhoInput(mocker, getGerarOrdemRetrabalhoInput, 0, ignoreList: new List<string>
        {
            nameof(GerarOrdemRetrabalhoInput.MateriaisInput),
            nameof(GerarOrdemRetrabalhoInput.MaquinasInput)
        });

        MockarExternalGerarOrdemRetrabalho(mocker, expectedInput: entidadeMockada,
            expectedOutput: new ExternalGerarOrdemRetrabalhoOutput
            {
                Success = true,
                OdfGerada = 901
            });

        mocker.LocalProvider.GetByCode(TestUtils.ObjectMother.Ints[0]).Returns(new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        });
        
        mocker.EstoqueLocalAclService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new EstoqueLocalOutput
        {
            CodigoLocal = TestUtils.ObjectMother.Ints[0],
            CodigoArmazem = TestUtils.ObjectMother.Ints[0].ToString(),
            DataFabricacao = TestUtils.ObjectMother.Datas[0],
            DataValidade = TestUtils.ObjectMother.Datas[0]
        });
        
        //Act
        var result = await service.GerarOrdemRetrabalho(agregacao, input);

        //Assert
        await mocker.PushNotification.Received(1).SendAsync(Arg.Is<Payload>(e =>
                e.Header == "Odf retrabalho gerada com sucesso" 
                && e.Body == $"Número odf retrabalho: 901"),
            TestUtils.ObjectMother.Guids[0], true);
    }
    
    private GerarOrdemRetrabalhoInput GetGerarOrdemRetrabalhoInput(int index)
    {
        var gerarOrdemRetrabalhoInput = new GerarOrdemRetrabalhoInput
        {
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalDestino = TestUtils.ObjectMother.Guids[index],
            IdProduto = TestUtils.ObjectMother.Guids[index],
            NumeroPedido = TestUtils.ObjectMother.Strings[index],
            IdPessoa = TestUtils.ObjectMother.Guids[index],
            NumeroOdfOrigem = TestUtils.ObjectMother.Ints[index],
            NumeroLote = TestUtils.ObjectMother.Strings[index]
        };
        return gerarOrdemRetrabalhoInput;
    }

    private ExternalGerarOrdemRetrabalhoInput MockarGetExternalGerarOrdemRetrabalhoInput(Mocker mocker, GerarOrdemRetrabalhoInput expectedInput, int index, 
            List<string> ignoreList)
    {
        var externalInput = new ExternalGerarOrdemRetrabalhoInput()
        {
            Quantidade = TestUtils.ObjectMother.Ints[index],
            CodigoProduto = TestUtils.ObjectMother.Strings[index],
            Lote = TestUtils.ObjectMother.Strings[index],
            AnalisarReversa = false,
            Retrabalho = true,
            Pedido = TestUtils.ObjectMother.Strings[index],
            DataEntrega = TestUtils.ObjectMother.Strings[index],
            CodigoCliente = TestUtils.ObjectMother.Strings[index],
            OdfOrigem = TestUtils.ObjectMother.Ints[index],
            Projetar = false,
            Servico = false,
            LocalDestino = TestUtils.ObjectMother.Ints[index],
            IdEmpresa = TestUtils.ObjectMother.Ints[index]
        };
        
        mocker.OrdemRetrabalhoAclService.GetExternalGerarOrdemRetrabalhoInput(Arg.Is<GerarOrdemRetrabalhoInput>(e =>
            e.IsEquivalentTo(expectedInput, ignoreList))).Returns(externalInput);
        return externalInput;
    }

    private void MockarExternalGerarOrdemRetrabalho(Mocker mocker, ExternalGerarOrdemRetrabalhoInput expectedInput, 
        ExternalGerarOrdemRetrabalhoOutput expectedOutput)
    {
        mocker.ExternalOrdemRetrabalhoService.GerarOrdemRetrabalho(expectedInput).Returns(expectedOutput);
    }
    
}