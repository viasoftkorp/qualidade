using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests;

public class UpdateTests : SolucaoServiceTest
{
    [Fact(DisplayName = "Update Solucao with Success")]
    public async Task UpdateSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucaoInput = TestUtils.ObjectMother.GetSolucao(0);

        await mocker.Solucoes.InsertAsync(solucaoInput);

        var updateInput = new SolucaoInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[2],
            Codigo = solucaoInput.Codigo,
            Detalhamento = TestUtils.ObjectMother.Strings[3]
        };

        await UnitOfWork.SaveChangesAsync();
        
        var expectedResult = new Solucao
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[2],
            Codigo = solucaoInput.Codigo,
            Detalhamento = TestUtils.ObjectMother.Strings[3],
            IsAtivo = true,
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0]
        };

        //Act
        var output = await service.Update(solucaoInput.Id, updateInput);

        //Assert
        var solucao = await mocker.Solucoes.FindAsync(updateInput.Id);

        solucao.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "Update Solucao Returns NotFound")]
    public async Task UpdateSolucaoWithNotFoundIdTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var updateInput = TestUtils.ObjectMother.GetSolucaoInput(0);

        //Act
        var output = await service.Update(updateInput.Id, updateInput);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
    
    [Fact(DisplayName = "Se isAtivo vier pelo input, o mesmo não deve ser alterado")]
    public async Task UpdateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var solucao = TestUtils.ObjectMother.GetSolucao(0);
        solucao.IsAtivo = true;
        await mocker.Solucoes.InsertAsync(solucao, true);
        
        var input = new SolucaoInput()
        {
            Id = solucao.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = solucao.Codigo,
            IsAtivo = false
        };

        //Act
        var output = await service.Update(solucao.Id, input);

        //Assert
        var result = await mocker.Solucoes.FindAsync(solucao.Id);

        result.IsAtivo.Should().BeTrue();

    }
}