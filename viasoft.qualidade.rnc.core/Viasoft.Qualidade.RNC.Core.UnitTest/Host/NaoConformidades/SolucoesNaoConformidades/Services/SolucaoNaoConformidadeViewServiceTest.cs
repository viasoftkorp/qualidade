using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public class SolucaoNaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "GetList solucao with Success")]
    public async Task GetListSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucaoNaoConformidade = TestUtils.ObjectMother.GetSolucaoNaoConformidade(0);
        var solucao = TestUtils.ObjectMother.GetSolucao(0);
        var usuario = TestUtils.ObjectMother.GetUsuario(0);
        await mocker.Usuario.InsertAsync(usuario);
        await mocker.SolucaoNaoConformidade.InsertAsync(solucaoNaoConformidade);
        await mocker.Solucao.InsertAsync(solucao);

        await UnitOfWork.SaveChangesAsync();

        var input = new GetListWithDefeitoIdFlagInput
        {
            MaxResultCount = 1,
            SkipCount = 0
        };
        var expected = new List<SolucaoNaoConformidadeViewOutput>{new(solucaoNaoConformidade, solucao, usuario)};

        //Act
        var output = await service
            .GetListView(TestUtils.ObjectMother.Guids[0], TestUtils.ObjectMother.Guids[0], input);

        //Assert
        output.TotalCount.Should().Be(1); 
        output.Items.Should().BeEquivalentTo(expected);
    }
    private SolucaoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new SolucaoNaoConformidadeServiceMocker()
        {
            SolucaoNaoConformidade = ServiceProvider.GetService<IRepository<SolucaoNaoConformidade>>(),
            Solucao = ServiceProvider.GetService<IRepository<Solucao>>(),
            Usuario = ServiceProvider.GetService<IRepository<Usuario>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private SolucaoNaoConformidadeViewService GetService(SolucaoNaoConformidadeServiceMocker mocker)
    {
        this.ConfigureServices();
        var service = new SolucaoNaoConformidadeViewService(mocker.SolucaoNaoConformidade, mocker.Solucao, mocker.Usuario,
            mocker.FakeCurrentCompany);
        return service;
    }

    public class SolucaoNaoConformidadeServiceMocker
    {
        public IRepository<SolucaoNaoConformidade> SolucaoNaoConformidade { get; set; }
        public IRepository<Solucao> Solucao { get; set; }
        public IRepository<Usuario> Usuario { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}