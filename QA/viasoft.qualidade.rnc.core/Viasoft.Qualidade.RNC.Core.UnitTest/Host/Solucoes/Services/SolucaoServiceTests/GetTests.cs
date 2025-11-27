using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests;

public class GetTests : SolucaoServiceTest
{
    [Fact(DisplayName = "Get Solucao with Success")]
    public async Task GetSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetSolucao(0);

        await mocker.Solucoes.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var solucao = await mocker.Solucoes.FindAsync(TestUtils.ObjectMother.Guids[0]);
        solucao.Should().BeEquivalentTo(output);
    }

    [Fact(DisplayName = "Get Solucao Returns Null")]
    public async Task GetSolucaoWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetSolucao(0);

        await mocker.Solucoes.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().BeNull();
    }

    [Fact(DisplayName = "GetList Solucao with Success")]
    public async Task GetListSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucao0 = TestUtils.ObjectMother.GetSolucao(0);
        var solucao1 = TestUtils.ObjectMother.GetSolucao(1);

        Solucao[] solucoes = { solucao0, solucao1 };

        await mocker.Solucoes.InsertRangeAsync(solucoes);

        await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput();
        //Act
        var output = await service.GetList(input);

        //Assert
        output.TotalCount.Should().Be(2);
        var firstItem = output.Items.First(e => e.Id == TestUtils.ObjectMother.Guids[0]);
        firstItem.Should().BeEquivalentTo(new SolucaoOutput(solucao0));
        
        var secondItem = output.Items.First(e => e.Id == TestUtils.ObjectMother.Guids[1]);
        secondItem.Should().BeEquivalentTo(new SolucaoOutput(solucao1));
    }
}