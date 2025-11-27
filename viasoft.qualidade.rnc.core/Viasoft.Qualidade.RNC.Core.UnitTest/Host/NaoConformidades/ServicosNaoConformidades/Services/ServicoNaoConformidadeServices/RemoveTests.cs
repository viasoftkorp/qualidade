using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Services.ServicoNaoConformidadeServices;

public class RemoveTests : ServicoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Remove ServicoSolucaoNaoConformidade with Success")]
    public async Task RemoveServicoSolucaoNaoConformidadeWithSuccessTest()
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
        };
        
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Remove(idNaoConformidade, servicoSolucaoInput.Id);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        var servicoSolucao = agregacao.ServicoNaoConformidades.Find(p => p.Id.Equals(servicoSolucaoInput.Id));
        servicoSolucao.Should().BeNull();
    }
}