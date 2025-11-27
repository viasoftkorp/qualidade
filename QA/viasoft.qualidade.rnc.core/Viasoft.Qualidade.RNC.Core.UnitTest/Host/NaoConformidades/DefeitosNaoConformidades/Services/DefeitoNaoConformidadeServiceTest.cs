using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public class DefeitoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get DefeitoNaoConformidade with Success")]
    public async Task GetDefeitoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetDefeitoNaoConformidade(0);
        
        await mocker.DefeitoNaoConformidade.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade, input.Id);

        //Assert
        var defeito = await mocker.DefeitoNaoConformidade.FindAsync(TestUtils.ObjectMother.Guids[0]);
        defeito.Should().BeEquivalentTo(output);
    }
    
    [Fact(DisplayName = "Get DefeitoNaoConformidade Returns Null")]
    public async Task GetDefeitoWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetDefeitoNaoConformidade(0);
        
        await mocker.DefeitoNaoConformidade.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade,TestUtils.ObjectMother.Guids[3]);

        //Assert
        output.Should().BeNull();
    }
    [Fact(DisplayName = "Insert DefeitoNaoConformidade with Success")]
    public async Task InsertDefeitoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var defeitoInput = new DefeitoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
        };
        var expectedResult = new DefeitoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Insert(idNaoConformidade, defeitoInput);
        //Assert
        var result = naoConformidade.DefeitoNaoConformidades.Find(p => p.Id.Equals(defeitoInput.Id));
        result.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);

    }

    [Fact(DisplayName = "Update DefeitoNaoConformidade with Success")]
    public async Task UpdateDefeitoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var defeitoInput = new DefeitoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0]
        };
        var expectedResult = new DefeitoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, defeitoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Update(idNaoConformidade, defeitoInput.Id, defeitoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(defeitoInput.IdNaoConformidade);
        var defeito = agregacao.DefeitoNaoConformidades.Find(p => p.IdNaoConformidade.Equals(idNaoConformidade));
        defeito.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Remove DefeitoNaoConformidade with Success")]
    public async Task RemoveDefeitoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var defeitoInput = new DefeitoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[2],
            IdNaoConformidade = idNaoConformidade,
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0]
        };

        await service.Insert(idNaoConformidade, defeitoInput);
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Remove(idNaoConformidade, defeitoInput.Id);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        var defeito = agregacao.DefeitoNaoConformidades.Find(p => p.Id.Equals(defeitoInput.Id));
        defeito.Should().BeNull();
    }

    private DefeitoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new DefeitoNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment =TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            DefeitoNaoConformidade = ServiceProvider.GetService<IRepository<DefeitoNaoConformidade>>(),
            CurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];

        return mocker;
    }

    private DefeitoNaoConformidadeService GetService(DefeitoNaoConformidadeServiceMocker mocker)
    {

        var service = new DefeitoNaoConformidadeService(mocker.NaoConformidadeRepository,
            mocker.DateTimeProvider, mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, 
            mocker.DefeitoNaoConformidade, mocker.CurrentCompany);

        return service;
    }

    public class DefeitoNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<DefeitoNaoConformidade> DefeitoNaoConformidade { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
    }
}