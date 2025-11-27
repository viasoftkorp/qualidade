using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Services;

public class ServicoNaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "GetList ServicoNaoConformidade with Success")]
    public async Task GetListServicoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var servicoNaoConformidade = TestUtils.ObjectMother.GetServicoNaoConformidade(0);
        var produto = TestUtils.ObjectMother.GetProduto(0);
        var recursos = TestUtils.ObjectMother.GetRecurso(0);
        await mocker.ServicoNaoConformidade.InsertAsync(servicoNaoConformidade);
        await mocker.Produto.InsertAsync(produto);
        await mocker.Recurso.InsertAsync(recursos);

        await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput
        {
            MaxResultCount = 1,
            SkipCount = 0
        };
        var expected = new List<ServicoNaoConformidadeViewOutput>{new(servicoNaoConformidade, produto, recursos)};

        //Act
        var output = await service
            .GetListView(TestUtils.ObjectMother.Guids[0], input);

        //Assert
        output.TotalCount.Should().Be(1);
        output.Items.Should().BeEquivalentTo(expected);
    }
  
    private ServicoNaoConformidadeserviceMocker GetMocker()
    {
        var mocker = new ServicoNaoConformidadeserviceMocker()
        {
            ServicoNaoConformidade = ServiceProvider.GetService<IRepository<ServicoNaoConformidade>>(),
            Produto = ServiceProvider.GetService<IRepository<Produto>>(),
            Recurso = ServiceProvider.GetService<IRepository<Recurso>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private ServicoNaoConformidadeViewService GetService(ServicoNaoConformidadeserviceMocker mocker)
    {
        this.ConfigureServices();
        var service = new ServicoNaoConformidadeViewService(mocker.ServicoNaoConformidade, mocker.Produto, mocker.Recurso,
            mocker.FakeCurrentCompany);
        return service;
    }

    public class ServicoNaoConformidadeserviceMocker
    {
        public IRepository<ServicoNaoConformidade> ServicoNaoConformidade { get; set; }
        public IRepository<Produto> Produto { get; set; }
        public IRepository<Recurso> Recurso { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}