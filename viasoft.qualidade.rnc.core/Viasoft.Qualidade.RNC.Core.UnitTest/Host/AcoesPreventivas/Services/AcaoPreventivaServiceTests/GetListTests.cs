using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public class GetListTests : AcaoPreventivaServiceTest
{
    [Fact(DisplayName = "Se houver ações preventivas, deve retorna-las paginando-as")]
    public async Task GetListTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var acaoPreventiva = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
        };
        await mocker.AcaoPreventiva.InsertAsync(acaoPreventiva, true);

        var expectedResult = new PagedResultDto<AcaoPreventivaOutput>
        {
            TotalCount = 1,
            Items = new List<AcaoPreventivaOutput>
            {
                new AcaoPreventivaOutput(acaoPreventiva)
            }
        };
        //Act
        var output = await service.GetList(new PagedFilteredAndSortedRequestInput());
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
}