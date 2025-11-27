using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Usuarios;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalEntities.Usuarios.Services;

public class UsuarioServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se usuario não cadastrado, deve cadastra-lo")]
    public async Task ProcessarTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        mocker.UsuarioProxyService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new UsuarioOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            FirstName = TestUtils.ObjectMother.Strings[0],
            SecondName = TestUtils.ObjectMother.Strings[0]
        });
        var expectedResult = new Usuario
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Nome = TestUtils.ObjectMother.Strings[0],
            Sobrenome = TestUtils.ObjectMother.Strings[0],
        };
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var usuarioCadastrado = await mocker.Usuarios.FindAsync(TestUtils.ObjectMother.Guids[0]);
        usuarioCadastrado.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    
    [Fact(DisplayName = "Se usuario já não cadastrado, nada deve acontecer")]
    public async Task ProcessarTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        await mocker.Usuarios.InsertAsync(new Usuario
        {
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);
        //Act
        await service.InserirSeNaoCadastrado(TestUtils.ObjectMother.Guids[0]);
        //Assert
        mocker.Usuarios.Count().Should().Be(1);
    }
    public class Mocker
    {
        public IRepository<Usuario> Usuarios { get; set; }
        public IUsuarioProxyService UsuarioProxyService { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            Usuarios = ServiceProvider.GetService<IRepository<Usuario>>(),
            UsuarioProxyService = Substitute.For<IUsuarioProxyService>()
        };
        return mocker;
    }

    protected UsuarioService GetService(Mocker mocker)
    {
        var service = new UsuarioService(mocker.Usuarios, mocker.UsuarioProxyService, UnitOfWork);

        return service;
    }
}