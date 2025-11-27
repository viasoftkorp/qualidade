using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Handlers.NaoConformidadeViewHandlerTests;

public class NaoConformidadeViewHandlerNaoConformidadeAtualizadaMessageTests : NaoConformidadeViewHandlerTest
{
    [Fact(DisplayName = "Se não tiver id pessoa, nenhum registro de pessoa deve ser feito")]
    public async Task NaoConformidadeAtualizadaMessageTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new NaoConformidadeAtualizada
        {
            NaoConformidade = new NaoConformidadeModel
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdProduto = TestUtils.ObjectMother.Guids[0],
                IdCriador = TestUtils.ObjectMother.Guids[0],
            }
        };

        await UnitOfWork.SaveChangesAsync();
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.PessoaService.ReceivedWithAnyArgs(0).InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se tiver id pessoa, deve chamar InserirPessoaSeNaoCadastrada")]
    public async Task NaoConformidadeAtualizadaMessageTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new NaoConformidadeAtualizada
        {
            NaoConformidade = new NaoConformidadeModel
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdPessoa = TestUtils.ObjectMother.Guids[0],
                IdProduto = TestUtils.ObjectMother.Guids[0],
                IdCriador = TestUtils.ObjectMother.Guids[0]
            }
        };

        await UnitOfWork.SaveChangesAsync();
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.PessoaService.Received(1).InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
    }

    [Fact(DisplayName = "Se método chamado, deve chamar InserirProdutoSeNaoCadastrado")]
    public async Task NaoConformidadeAtualizadaMessageTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new NaoConformidadeAtualizada
        {
            NaoConformidade = new NaoConformidadeModel
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdPessoa = TestUtils.ObjectMother.Guids[0],
                IdProduto = TestUtils.ObjectMother.Guids[0],
                IdCriador = TestUtils.ObjectMother.Guids[0]
            }
        };
        
        await UnitOfWork.SaveChangesAsync();
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.ProdutoService.Received(1).InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
    }
    
    [Fact(DisplayName = "Se houver nota fiscal e origem igual a inspeção de entrada, deve buscar número da nota fiscal de entrada ")]
    public async Task NaoConformidadeAtualizadaMessageTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new NaoConformidadeAtualizada
        {
            NaoConformidade = new NaoConformidadeModel
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdPessoa = TestUtils.ObjectMother.Guids[0],
                IdProduto = TestUtils.ObjectMother.Guids[0],
                IdCriador = TestUtils.ObjectMother.Guids[0],
                IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
                Origem = OrigemNaoConformidade.InspecaoEntrada
            }
        };
        
        var naoConformidade = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            CreationTime = TestUtils.ObjectMother.Datas[0],
            CreatorId = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = null
        };
        var expectedResult = new NaoConformidade()
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            CreationTime = TestUtils.ObjectMother.Datas[0],
            CreatorId = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = "152"
        };
        
        mocker.FakeNotaFiscalEntradaProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new NotaFiscalEntradaOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            CodigoFornecedor = TestUtils.ObjectMother.Ints[0].ToString(),
            IdFornecedor = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = 152,
            Lote = "1"
        });
        await mocker.NaoConformidadesRepository.InsertAsync(naoConformidade);
        await UnitOfWork.SaveChangesAsync();
        //Act
        await handler.Handle(message);
        //Assert
        var naoConformidadeAtualizada = await mocker.NaoConformidadesRepository.FindAsync(TestUtils.ObjectMother.Guids[0]);
        naoConformidadeAtualizada.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);

        await mocker.FakeNotaFiscalSaidaProvider.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }
    
    [Fact(DisplayName = "Se houver nota fiscal e origem diferente de inspeção de entrada, deve buscar número da nota fiscal de saida ")]
    public async Task NaoConformidadeAtualizadaMessageTest5()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new NaoConformidadeAtualizada
        {
            NaoConformidade = new NaoConformidadeModel
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdPessoa = TestUtils.ObjectMother.Guids[0],
                IdProduto = TestUtils.ObjectMother.Guids[0],
                IdCriador = TestUtils.ObjectMother.Guids[0],
                IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
                Origem = OrigemNaoConformidade.InpecaoSaida
            }
        };
        
        var naoConformidade = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            CreationTime = TestUtils.ObjectMother.Datas[0],
            CreatorId = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = null
        };
        var expectedResult = new NaoConformidade()
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            CreationTime = TestUtils.ObjectMother.Datas[0],
            CreatorId = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = "152"
        };
        
        mocker.FakeNotaFiscalSaidaProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new NotaFiscalSaidaOutput()
        {
            Id = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = 152
        });
        await mocker.NaoConformidadesRepository.InsertAsync(naoConformidade);
        await UnitOfWork.SaveChangesAsync();
        //Act
        await handler.Handle(message);
        //Assert
        var naoConformidadeAtualizada = await mocker.NaoConformidadesRepository.FindAsync(TestUtils.ObjectMother.Guids[0]);
        naoConformidadeAtualizada.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);

        await mocker.FakeNotaFiscalEntradaProvider.ReceivedWithAnyArgs(0).GetById(TestUtils.ObjectMother.Guids[0]);
    }

    
}
