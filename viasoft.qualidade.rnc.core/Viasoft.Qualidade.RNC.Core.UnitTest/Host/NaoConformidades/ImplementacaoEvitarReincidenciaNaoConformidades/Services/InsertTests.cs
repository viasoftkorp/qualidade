using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.
    Services;

public class InsertTests : ImplementacaoEvitarReincidenciaNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Se método chamado, deve inserir nova implementação")]
    public async Task InsertTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new ImplementacaoEvitarReincidenciaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[1],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[1],
            Descricao = TestUtils.ObjectMother.Strings[1],
            AcaoImplementada = true,
            DataAnalise = TestUtils.ObjectMother.Datas[1],
            DataVerificacao = TestUtils.ObjectMother.Datas[1],
            IdAuditor = TestUtils.ObjectMother.Guids[1],
            IdResponsavel = TestUtils.ObjectMother.Guids[1],
            NovaData = TestUtils.ObjectMother.Datas[1],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[1],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[1]
        };
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var expectedResult = new ImplementacaoEvitarReincidenciaNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[1],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[1],
            Descricao = TestUtils.ObjectMother.Strings[1],
            AcaoImplementada = true,
            DataAnalise = TestUtils.ObjectMother.Datas[1],
            DataVerificacao = TestUtils.ObjectMother.Datas[1],
            IdAuditor = TestUtils.ObjectMother.Guids[1],
            IdResponsavel = TestUtils.ObjectMother.Guids[1],
            NovaData = TestUtils.ObjectMother.Datas[1],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[1],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[1],
            CompanyId = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        await service.Insert(TestUtils.ObjectMother.Guids[0], input);
        //Assert
        var result =
            naoConformidade.ImplementacoesEvitarReincidenciaNaoConformidades.Find(p => p.Id.Equals(TestUtils.ObjectMother.Guids[1]));
        result.Should().BeEquivalentTo(expectedResult);
    }
}