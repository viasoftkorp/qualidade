using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public class AcaoPreventivaNaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "GetList AcaoPreventiva with Success")]
    public async Task GetListAcaoPreventivaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var acaoPreventivaNaoConformidade = TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidade(0);
        var acaoPreventiva = TestUtils.ObjectMother.GetAcaoPreventiva(0);;
        var usuario = TestUtils.ObjectMother.GetUsuario(0);
        await mocker.AcaoPreventivaNaoConformidade.InsertAsync(acaoPreventivaNaoConformidade);
        await mocker.AcaoPreventiva.InsertAsync(acaoPreventiva);
        await mocker.Usuario.InsertAsync(usuario);
        
        await UnitOfWork.SaveChangesAsync();

        var input = new GetListWithDefeitoIdFlagInput
        {
            MaxResultCount = 1,
            SkipCount = 0
        };
        var expected = new List<AcaoPreventivaNaoConformidadeViewOutput>{new(acaoPreventivaNaoConformidade, acaoPreventiva, usuario)};

        //Act
        var output = await service
            .GetListView(TestUtils.ObjectMother.Guids[0], TestUtils.ObjectMother.Guids[0], input);

        //Assert
        output.TotalCount.Should().Be(1);
        output.Items.Should().BeEquivalentTo(expected);
    }
    private AcaoPreventivaNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new AcaoPreventivaNaoConformidadeServiceMocker()
        {
            AcaoPreventivaNaoConformidade = ServiceProvider.GetService<IRepository<AcaoPreventivaNaoConformidade>>(),
            AcaoPreventiva = ServiceProvider.GetService<IRepository<AcaoPreventiva>>(),
            Usuario = ServiceProvider.GetService<IRepository<Usuario>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private AcaoPreventivaNaoConformidadeViewService GetService(AcaoPreventivaNaoConformidadeServiceMocker mocker)
    {
        this.ConfigureServices();
        var service = new AcaoPreventivaNaoConformidadeViewService(mocker.AcaoPreventivaNaoConformidade, mocker.AcaoPreventiva, mocker.Usuario, mocker.FakeCurrentCompany);
        return service;
    }

    public class AcaoPreventivaNaoConformidadeServiceMocker
    {
        public IRepository<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidade { get; set; }
        public IRepository<AcaoPreventiva> AcaoPreventiva { get; set; }
        public IRepository<Usuario> Usuario { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}