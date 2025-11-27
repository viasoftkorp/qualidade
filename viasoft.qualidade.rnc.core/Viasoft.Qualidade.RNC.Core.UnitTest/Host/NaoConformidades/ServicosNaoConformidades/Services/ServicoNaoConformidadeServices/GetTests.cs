using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Services.ServicoNaoConformidadeServices;

public class GetTests : ServicoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Get ServicoNaoConformidade with Success")]
    public async Task GetServicoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetServicoNaoConformidade(0);
        
        await mocker.ServicoNaoConformidadeRepository.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade, input.Id);

        //Assert
        var produtoSolucao = await mocker.ServicoNaoConformidadeRepository.FindAsync(TestUtils.ObjectMother.Guids[0]);
        produtoSolucao.Should().BeEquivalentTo(output, options => options.Excluding(e=> e.CompanyId));
    }
    
    [Fact(DisplayName = "Get ServicoSolucaoNaoConformidade Returns Null")]
    public async Task GetServicoSolucaoWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetServicoNaoConformidade(0);
        
        await mocker.ServicoNaoConformidadeRepository.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade,TestUtils.ObjectMother.Guids[3]);

        //Assert
        output.Should().BeNull();
    }
}