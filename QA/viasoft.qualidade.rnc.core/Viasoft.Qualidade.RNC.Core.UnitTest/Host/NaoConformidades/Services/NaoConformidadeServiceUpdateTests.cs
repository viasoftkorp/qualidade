using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services;

public class NaoConformidadeServiceUpdateTests : NaoConformidadeServiceTest
{
    [Fact(DisplayName = "Update NaoConformidade with Success")]
    public async Task UpdateNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.AgregacaoRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);

        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = 0,
            Status = 0,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[1],
            IdPessoa = TestUtils.ObjectMother.Guids[1],
            IdProduto = TestUtils.ObjectMother.Guids[1],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[1],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "2",
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
            IdNatureza = TestUtils.ObjectMother.Guids[1],
            IdPessoa = TestUtils.ObjectMother.Guids[1],
            IdProduto = TestUtils.ObjectMother.Guids[1],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[1],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "2",
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
            CompanyId = TestUtils.ObjectMother.Guids[0],
            DataCriacao = TestUtils.ObjectMother.Datas[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Update(TestUtils.ObjectMother.Guids[0], input);
        //Assert
        ServiceBus.FakeBus.Events.OfType<MessagePublished<NaoConformidadeAtualizada>>().Should().HaveCount(1);
        agregacaoCriada.NaoConformidadeAlterar.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    
    [Fact(DisplayName = "Se não conformidade for invalida, deve retornar mensagem de erro")]
    public async Task UpdateInvalidNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.AgregacaoRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);

        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = OrigemNaoConformidade.InpecaoSaida,
            Status = 0,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[1],
            IdPessoa = null,
            IdProduto = TestUtils.ObjectMother.Guids[1],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[1],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "2",
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
        mocker.FakeNaoConformidadeValidationService.ValidarCampoCliente(Arg.Is<NaoConformidadeInput>(e =>
                e.Id == TestUtils.ObjectMother.Guids[0]
                && e.Origem == OrigemNaoConformidade.InpecaoSaida
                && e.IdPessoa == null))
            .Returns(NaoConformidadeValidationResult.ClienteObrigatorio);
        //Act
        var result = await service.Update(TestUtils.ObjectMother.Guids[0], input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.ClienteObrigatorio);
    }
}