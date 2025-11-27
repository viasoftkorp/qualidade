using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Rebus.TestHelpers.Events;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.Utils.Consts;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services;

public class NaoConformidadeServiceCreateTests : NaoConformidadeServiceTest
{
    [Fact(DisplayName = "Create NaoConformidade with Success")]
    public async Task CreateNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        await agregacaoCriada.SalvarNaoConformidade(mocker.AgregacaoRepository);

        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = 0,
            Status = 0,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = TestUtils.ObjectMother.Strings[0],
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0]
        };
        var expectedResult = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = 0,
            Status = 0,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            DataCriacao = TestUtils.ObjectMother.Datas[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = TestUtils.ObjectMother.Strings[0],
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            NumeroNotaFiscal = TestUtils.ObjectMother.Ints[0].ToString(),
            NumeroLote = TestUtils.ObjectMother.Ints[0].ToString(),
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
            NumeroPedido = TestUtils.ObjectMother.Strings[0],
            NumeroOdfFaturamento = TestUtils.ObjectMother.Ints[0],
            IdProdutoFaturamento = TestUtils.ObjectMother.Guids[0]
        };

        mocker.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        
        //Act
        var result = await service.Create(input);
        //Assert
        ServiceBus.FakeBus.Events.OfType<MessagePublished<NaoConformidadeInserida>>().Should().HaveCount(1);
        naoConformidade.NaoConformidade.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }


    [Fact(DisplayName = "Se não conformidade for invalida, deve retornar mensagem de erro")]
    public async Task CreateInvalidNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        await agregacaoCriada.SalvarNaoConformidade(mocker.AgregacaoRepository);

        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = OrigemNaoConformidade.InpecaoSaida,
            Status = 0,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = null,
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = TestUtils.ObjectMother.Strings[0],
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0]
        };
        mocker.FakeNaoConformidadeValidationService
            .ValidarCampoCliente(Arg.Is<NaoConformidadeInput>(e => e.Id == TestUtils.ObjectMother.Guids[0]
                                                                   && e.IdPessoa == null
                                                                   && e.Origem == OrigemNaoConformidade.InpecaoSaida))
            .Returns(NaoConformidadeValidationResult.ClienteObrigatorio);
        //Act
        var result = await service.Create(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.ClienteObrigatorio);
    }
    
    [Fact(DisplayName = "Se não conformidade for incompleta, deve adiar uma mensagem de VerificarNaoConformidadeIncompleta em 24hrs")]
    public async Task CreateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        await agregacaoCriada.SalvarNaoConformidade(mocker.AgregacaoRepository);

        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = 0,
            Status = 0,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = TestUtils.ObjectMother.Strings[0],
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            Incompleta = true
        };

        var expectedResult = new VerificarNaoConformidadeIncompletaCommand
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0]
        };
        
        //Act
        var result = await service.Create(input);
        //Assert
        var message = ServiceBus.FakeBus.Events.OfType<MessageDeferredToSelf<VerificarNaoConformidadeIncompletaCommand>>().First();
        message.CommandMessage.Should().BeEquivalentTo(expectedResult);
        message.Delay.Should().Be(new TimeSpan(24, 0, 0));
    }
}