using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Services.ServicoNaoConformidadeServices;

public class InsertTests : ServicoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Insert ServicoSolucaoNaoConformidade with Success")]
    public async Task InsertServicoSolucaoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        agregacaoCriada.ServicoNaoConformidades.Clear();
        
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        MockValidarTempo(TestUtils.ObjectMother.Ints[0], TestUtils.ObjectMother.Ints[0], true);

        var servicoSolucaoInput = new ServicoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Horas = TestUtils.ObjectMother.Ints[0],
            Minutos = TestUtils.ObjectMother.Ints[0],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0],
            Detalhamento =  TestUtils.ObjectMother.Strings[0],
        };
        var expectedResult = new ServicoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Horas = TestUtils.ObjectMother.Ints[0],
            Minutos = TestUtils.ObjectMother.Ints[0],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0],
            Detalhamento =  TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        await service.Insert(idNaoConformidade, servicoSolucaoInput);
        //Assert
        var result = naoConformidade.ServicoNaoConformidades.Find(p => p.Id.Equals(servicoSolucaoInput.Id));
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se falhar ao validar tempo, deve retornar TempoInvalido")]
    public async Task InsertTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var servicoSolucaoInput = new ServicoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Horas = TestUtils.ObjectMother.Ints[0],
            Minutos = TestUtils.ObjectMother.Ints[0],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0],
            Detalhamento =  TestUtils.ObjectMother.Strings[0],
        };
        
        MockValidarTempo(TestUtils.ObjectMother.Ints[0], TestUtils.ObjectMother.Ints[0], false);

        //Act
        var output = await service.Insert(idNaoConformidade, servicoSolucaoInput);
        //Assert
        output.Should().Be(ServicoValidationResult.TempoInvalido);
    }
}