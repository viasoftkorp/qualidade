using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public class DefeitoNaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
   
    [Fact(DisplayName = "GetList Defeito with Success")]
    public async Task GetListDefeitoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var defeitoNaoConformidade = TestUtils.ObjectMother.GetDefeitoNaoConformidade(0);
        var defeito = TestUtils.ObjectMother.GetDefeito(0);
        await mocker.DefeitoNaoConformidade.InsertAsync(defeitoNaoConformidade);
        await mocker.Defeito.InsertAsync(defeito);

        await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput
        {
            MaxResultCount = 1,
            SkipCount = 0
        };
        var expected = new List<DefeitoNaoConformidadeViewOutput>{new(defeitoNaoConformidade, defeito)};

        //Act
        var output = await service
            .GetListView(TestUtils.ObjectMother.Guids[0], input);

        //Assert
        output.TotalCount.Should().Be(1);
        output.Items.Should().BeEquivalentTo(expected);
    }
    
    private DefeitoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new DefeitoNaoConformidadeServiceMocker()
        {
            DefeitoNaoConformidade = ServiceProvider.GetService<IRepository<DefeitoNaoConformidade>>(),
            Defeito = ServiceProvider.GetService<IRepository<Defeito>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
            
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private DefeitoNaoConformidadeViewService GetService(DefeitoNaoConformidadeServiceMocker mocker)
    {
        this.ConfigureServices();
        var service = new DefeitoNaoConformidadeViewService(mocker.DefeitoNaoConformidade, mocker.Defeito, mocker.FakeCurrentCompany);
        return service;
    }

    public class DefeitoNaoConformidadeServiceMocker
    {
        public IRepository<DefeitoNaoConformidade> DefeitoNaoConformidade { get; set; }
        public IRepository<Defeito> Defeito { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}