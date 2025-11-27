using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;

public class GetTests : CausaServiceTest
{
    [Fact(DisplayName = "Get Causa with Success")]
    public async Task GetCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetCausa(0);
        
        await mocker.Causas.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var causa = await mocker.Causas.FindAsync(TestUtils.ObjectMother.Guids[0]);
        causa.Should().BeEquivalentTo(output);
    }
    
    
    [Fact(DisplayName = "Get Causa Returns Null")]
    public async Task GetCausaWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetCausa(0);
        
        await mocker.Causas.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().BeNull();
    }


    [Fact(DisplayName = "GetList Causa with Success")]
    public async Task GetListCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa0 = TestUtils.ObjectMother.GetCausa(0);
        var causa1 = TestUtils.ObjectMother.GetCausa(1);
        
        Causa[] causas = { causa0,causa1 };
        
       await mocker.Causas.InsertRangeAsync(causas);
        
       await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput();

        //Act
        var output = await service.GetList(input);

        //Assert
        output.TotalCount.Should().Be(2);
        var firstItem = output.Items.First(e => e.Id == TestUtils.ObjectMother.Guids[0]);
        firstItem.Should().BeEquivalentTo(new CausaOutput(causa0));
        
        var secondItem = output.Items.First(e => e.Id == TestUtils.ObjectMother.Guids[1]);
        secondItem.Should().BeEquivalentTo(new CausaOutput(causa1));
    }
}