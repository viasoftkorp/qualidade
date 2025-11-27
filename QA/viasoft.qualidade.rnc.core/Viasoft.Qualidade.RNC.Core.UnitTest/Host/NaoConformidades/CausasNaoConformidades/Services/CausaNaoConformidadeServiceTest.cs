using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.CausasNaoConformidades.Services;

public class CausaNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get CausaNaoConformidade with Success")]
    public async Task GetCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetCausaNaoConformidade(0);
        
        await mocker.CausaNaoConformidades.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade, input.Id);

        //Assert
        var causa = await mocker.CausaNaoConformidades.FindAsync(TestUtils.ObjectMother.Guids[0]);
        causa.Should().BeEquivalentTo(output);
    }
    
    
    [Fact(DisplayName = "Get CausaNaoConformidade Returns Null")]
    public async Task GetCausaWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetCausaNaoConformidade(0);
        
        await mocker.CausaNaoConformidades.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade,TestUtils.ObjectMother.Guids[3]);

        //Assert
        output.Should().BeNull();
    }
    [Fact(DisplayName = "Insert CausaNaoConformidade with Success")]
    public async Task InsertCausaNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var causaInput = new CausaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
        };
        var expectedResult = new CausaNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Insert(idNaoConformidade, causaInput);
        //Assert
        var result = naoConformidade.CausaNaoConformidades.Find(p => p.Id.Equals(causaInput.Id));
        result.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);

    }

    [Fact(DisplayName = "Update CausaNaoConformidade with Success")]
    public async Task UpdateCausaNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var causaInput = new CausaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, causaInput);

        await UnitOfWork.SaveChangesAsync();

        var expectedResult = new CausaNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Update(idNaoConformidade, causaInput.Id, causaInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(causaInput.IdNaoConformidade);
        var causa = agregacao.CausaNaoConformidades.Find(p => p.IdNaoConformidade.Equals(idNaoConformidade));
        causa.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Remove CausaNaoConformidade with Success")]
    public async Task RemoveCausaNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var causaInput = new CausaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdCausa = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, causaInput);
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Remove(idNaoConformidade, causaInput.Id);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        var causa = agregacao.CausaNaoConformidades.Find(p => p.Id.Equals(causaInput.Id));
        causa.Should().BeNull();
    }

    private CausaNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new CausaNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment =TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            CausaNaoConformidades = ServiceProvider.GetService<IRepository<CausaNaoConformidade>>(),
            CurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private CausaNaoConformidadeService GetService(CausaNaoConformidadeServiceMocker mocker)
    {

        var service = new CausaNaoConformidadeService(mocker.NaoConformidadeRepository,
            mocker.DateTimeProvider, mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, 
            mocker.CausaNaoConformidades, mocker.CurrentCompany);

        return service;
    }

    public class CausaNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<CausaNaoConformidade> CausaNaoConformidades { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
    }
}