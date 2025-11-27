using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public class AcaoPreventivaNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get AcoesPreventivasNaoConformidade with Success")]
    public async Task GetAcoesPreventivasWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidade(0);
        
        await mocker.AcaoPreventiva.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade, input.Id);

        //Assert
        var causa = await mocker.AcaoPreventiva.FindAsync(TestUtils.ObjectMother.Guids[0]);
        causa.Should().BeEquivalentTo(output, options => options.Excluding(e => e.CompanyId));
    }
    
    
    [Fact(DisplayName = "Get AcoesPreventivasNaoConformidade Returns Null")]
    public async Task GetAcoesPreventivasWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidade(0);
        
        await mocker.AcaoPreventiva.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade,TestUtils.ObjectMother.Guids[3]);

        //Assert
        output.Should().BeNull();
    }
    [Fact(DisplayName = "Insert AcaoPreventivaNaoConformidade with Success")]
    public async Task InsertAcaoPreventivaNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var acaoInput = new AcaoPreventivaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdAcaoPreventiva = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            Acao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Implementada = false,
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Insert(idNaoConformidade, acaoInput);
        //Assert
        var result = naoConformidade.AcaoPreventivaNaoConformidades.Find(p => p.Id.Equals(acaoInput.Id));
        result.Should().BeEquivalentTo(acaoInput, options => options.Excluding(e => e.CompanyId));

    }

    [Fact(DisplayName = "Update AcaoPreventivaNaoConformidade with Success")]
    public async Task UpdateAcaoPreventivaNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var acaoInput = new AcaoPreventivaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdAcaoPreventiva = TestUtils.ObjectMother.Guids[0],
            Acao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Implementada = false,
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0]
        };
        var expectedResult = new AcaoPreventivaNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            IdAcaoPreventiva = TestUtils.ObjectMother.Guids[0],
            Acao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Implementada = false,
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, acaoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Update(idNaoConformidade, acaoInput.Id, acaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(acaoInput.IdNaoConformidade);
        var acao = agregacao.AcaoPreventivaNaoConformidades.Find(p => p.IdNaoConformidade.Equals(idNaoConformidade));
        acao.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Remove AcaoPreventivaNaoConformidade with Success")]
    public async Task RemoveAcaoPreventivaNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var acaoInput = new AcaoPreventivaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdAcaoPreventiva = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = idNaoConformidade,
            Acao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Implementada = false,
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
        };
        
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Remove(idNaoConformidade, TestUtils.ObjectMother.Guids[0]);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        var acao = agregacao.AcaoPreventivaNaoConformidades.Find(p => p.Id.Equals(acaoInput.Id));
        acao.Should().BeNull();
    }

    private AcaoPreventivaNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new AcaoPreventivaNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment =TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            AcaoPreventiva = ServiceProvider.GetService<IRepository<AcaoPreventivaNaoConformidade>>(),
            CurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private AcaoPreventivaNaoConformidadeService GetService(AcaoPreventivaNaoConformidadeServiceMocker mocker)
    {

        var service = new AcaoPreventivaNaoConformidadeService(mocker.NaoConformidadeRepository,
            mocker.DateTimeProvider, mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, 
            mocker.AcaoPreventiva, mocker.CurrentCompany);

        return service;
    }

    public class AcaoPreventivaNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<AcaoPreventivaNaoConformidade> AcaoPreventiva { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
    }
}