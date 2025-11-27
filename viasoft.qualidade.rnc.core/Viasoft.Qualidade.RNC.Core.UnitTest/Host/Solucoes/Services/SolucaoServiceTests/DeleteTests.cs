using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests;

public class DeleteTests : SolucaoServiceTest
{
    [Fact(DisplayName = "Delete Solucao with Success")]
    public async Task DeleteSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucaoInserida = TestUtils.ObjectMother.GetSolucao(0);

        await mocker.Solucoes.InsertAsync(solucaoInserida);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Delete(solucaoInserida.Id);

        //Assert
        var solucaoEncontrada = await mocker.Solucoes.AnyAsync(s => s.Id == solucaoInserida.Id);

        solucaoEncontrada.Should().BeFalse();
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "Delete Solucao with NotFound Id")]
    public async Task DeleteSolucaoWithNotFoundId()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
    
    [Fact(DisplayName = "Se solução utilizada por um defeito, deve retornar EntidadeEmUso")]
    public async Task DeleteTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucao = new Solucao
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        };

        await mocker.Solucoes.InsertAsync(solucao, true);
        await mocker.Defeitos.InsertAsync(new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        }, true);
        
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);
        mocker.Solucoes.Should().NotBeEmpty();
    }
    
    [Fact(DisplayName = "Se solução utilizada por uma não conformidade, deve retornar EntidadeEmUso")]
    public async Task DeleteTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucao = new Solucao
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        };

        await mocker.Solucoes.InsertAsync(solucao, true);
        
        await mocker.SolucaoNaoConformidades.InsertAsync(new SolucaoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0]
        }, true);
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);
        mocker.Solucoes.Should().NotBeEmpty();
    }
}