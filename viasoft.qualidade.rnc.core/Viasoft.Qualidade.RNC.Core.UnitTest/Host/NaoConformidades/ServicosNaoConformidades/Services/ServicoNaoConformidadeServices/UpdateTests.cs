using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Services.ServicoNaoConformidadeServices;

public class UpdateTests : ServicoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Update ServicoSolucaoNaoConformidade with Success")]
    public async Task UpdateServicoSolucaoNaoConformidadeWithSuccessTest()
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
            Quantidade = TestUtils.ObjectMother.Ints[1],
            Horas = TestUtils.ObjectMother.Ints[1],
            Minutos = TestUtils.ObjectMother.Ints[1],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[1],
            Detalhamento = TestUtils.ObjectMother.Strings[0]
        };
        MockValidarTempo(TestUtils.ObjectMother.Ints[1], TestUtils.ObjectMother.Ints[1], true);
        var expectedResult = new ServicoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[1],
            Horas = TestUtils.ObjectMother.Ints[1],
            Minutos = TestUtils.ObjectMother.Ints[1],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[1],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, servicoSolucaoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Update(idNaoConformidade, servicoSolucaoInput.Id, servicoSolucaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(servicoSolucaoInput.IdNaoConformidade);
        var servicoSolucao =
            agregacao.ServicoNaoConformidades.Find(p => p.IdNaoConformidade.Equals(idNaoConformidade));
        servicoSolucao.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }
    
    [Fact(DisplayName = "Se falhar ao validar tempo, deve retornar TempoInvalido")]
    public async Task UpdateTest2()
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
            Quantidade = TestUtils.ObjectMother.Ints[1],
            Horas = TestUtils.ObjectMother.Ints[1],
            Minutos = TestUtils.ObjectMother.Ints[1],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[1],
            Detalhamento = TestUtils.ObjectMother.Strings[0]
        };
        MockValidarTempo(TestUtils.ObjectMother.Ints[1], TestUtils.ObjectMother.Ints[1], false);

        await service.Insert(idNaoConformidade, servicoSolucaoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Update(idNaoConformidade, servicoSolucaoInput.Id, servicoSolucaoInput);

        //Assert
        output.Should().Be(ServicoValidationResult.TempoInvalido);
    }
}