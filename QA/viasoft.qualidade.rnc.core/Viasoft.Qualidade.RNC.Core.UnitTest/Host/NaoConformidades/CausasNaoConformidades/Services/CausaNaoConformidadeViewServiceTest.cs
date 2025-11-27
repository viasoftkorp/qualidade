using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.CausasNaoConformidades.Services;

public class CausaNaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "GetList Causa with Success")]
    public async Task GetListCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causaNaoConformidade = TestUtils.ObjectMother.GetCausaNaoConformidade(0);
        var causa = TestUtils.ObjectMother.GetCausa(0);
        await mocker.CausaNaoConformidade.InsertAsync(causaNaoConformidade);
        await mocker.Causa.InsertAsync(causa);

        await UnitOfWork.SaveChangesAsync();

        var input = new GetListWithDefeitoIdFlagInput
        {
            MaxResultCount = 1,
            SkipCount = 0
        };
        var expected = new List<CausaNaoConformidadeViewOutput>{new(causaNaoConformidade, causa)};

        //Act
        var output = await service
            .GetListView(TestUtils.ObjectMother.Guids[0], TestUtils.ObjectMother.Guids[0], input);

        //Assert
        output.TotalCount.Should().Be(1);
        output.Items.Should().BeEquivalentTo(expected);
    }
    private CausaNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new CausaNaoConformidadeServiceMocker()
        {
            CausaNaoConformidade = ServiceProvider.GetService<IRepository<CausaNaoConformidade>>(),
            Causa = ServiceProvider.GetService<IRepository<Causa>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private CausaNaoConformidadeViewService GetService(CausaNaoConformidadeServiceMocker mocker)
    {
        this.ConfigureServices();
        var service = new CausaNaoConformidadeViewService(mocker.CausaNaoConformidade, mocker.Causa, mocker.FakeCurrentCompany);
        return service;
    }

    public class CausaNaoConformidadeServiceMocker
    {
        public IRepository<CausaNaoConformidade> CausaNaoConformidade { get; set; }
        public IRepository<Causa> Causa { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}