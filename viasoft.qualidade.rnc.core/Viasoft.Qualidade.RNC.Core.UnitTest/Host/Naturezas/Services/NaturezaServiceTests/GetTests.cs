using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Services.NaturezaServiceTests;

public class GetTests : NaturezaServiceTest
{
    [Fact(DisplayName = "Get Natureza with Success")]
    public async Task GetNaturezaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var naturezaInput = TestUtils.ObjectMother.GetNatureza(0);

        await mocker.Naturezas.InsertAsync(naturezaInput);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(naturezaInput.Id);
       
        //Assert
        var natureza =  await mocker.Naturezas.FindAsync(output.Id);

        natureza.Should().BeEquivalentTo(naturezaInput);
    }


    [Fact(DisplayName = "Get Natureza Returns Null")]
    public async Task GetNaturezaReturnsNullTest()
    {
        //Arrange
        var mock = GetMocker();
        var service = GetService(mock);

        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        output.Should().BeNull();
    }

    
    [Fact(DisplayName = "GetList Natureza with Success")]
    public async Task GetListNaturezaWithSuccessTest()
    {
        //Arrange
        var mock = GetMocker();
        var service = GetService(mock);

        var natureza0 = TestUtils.ObjectMother.GetNatureza(0);
        var natureza1 = TestUtils.ObjectMother.GetNatureza(1);
      
        Natureza[] naturezas = {natureza0, natureza1};

        await mock.Naturezas.InsertRangeAsync(naturezas);
        await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput();

        //Act
        var output = await service.GetList(input);
     
        //Assert
        output.TotalCount.Should().Be(2);
        var firstItem = output.Items.First(e => e.Id == TestUtils.ObjectMother.Guids[0]);
        firstItem.Should().BeEquivalentTo(new NaturezaOutput(natureza0));
        
        var secondItem = output.Items.First(e => e.Id == TestUtils.ObjectMother.Guids[1]);
        secondItem.Should().BeEquivalentTo(new NaturezaOutput(natureza1));
    }
}