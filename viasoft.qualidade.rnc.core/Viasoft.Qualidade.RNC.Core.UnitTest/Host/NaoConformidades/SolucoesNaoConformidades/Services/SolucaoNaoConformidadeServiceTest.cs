using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public class SolucaoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get SolucaoNaoConformidade with Success")]
    public async Task GetSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetSolucaoNaoConformidade(0);
        
        await mocker.SolucaoNaoConformidade.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade, input.Id);

        //Assert
        var solucao = await mocker.SolucaoNaoConformidade.FindAsync(TestUtils.ObjectMother.Guids[0]);
        solucao.Should().BeEquivalentTo(output, options => options.Excluding(e => e.CompanyId));
    }
    
    
    [Fact(DisplayName = "Get SolucaoNaoConformidade Returns Null")]
    public async Task GetSolucaoWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //inserir 
        var input = TestUtils.ObjectMother.GetSolucaoNaoConformidade(0);
        
        await mocker.SolucaoNaoConformidade.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade,TestUtils.ObjectMother.Guids[3]);

        //Assert
        output.Should().BeNull();
    }
    [Fact(DisplayName = "Insert SolucaoNaoConformidade with Success")]
    public async Task InsertSolucaoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        naoConformidade.SolucaoNaoConformidades.Clear();
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var solucaoInput = new SolucaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = true,
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = TestUtils.ObjectMother.Decimals[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
        };
        var expectedResult = new SolucaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = true,
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = TestUtils.ObjectMother.Decimals[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        await service.Insert(idNaoConformidade, solucaoInput);
        //Assert
        var result = naoConformidade.SolucaoNaoConformidades.Find(p => p.Id.Equals(solucaoInput.Id));
        result.Should().BeEquivalentTo(expectedResult);

    }

    [Fact(DisplayName = "Update SolucaoNaoConformidade with Success")]
    public async Task UpdateSolucaoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var solucaoInput = new SolucaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = false,
            DataAnalise = default,
            DataPrevistaImplantacao = default,
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = 0,
            NovaData = default,
            DataVerificacao = default,
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
        };
        var expectedResult = new SolucaoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = false,
            DataAnalise = default,
            DataPrevistaImplantacao = default,
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = 0,
            NovaData = default,
            DataVerificacao = default,
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };

        await service.Insert(idNaoConformidade, solucaoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Update(idNaoConformidade, solucaoInput.Id, solucaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(solucaoInput.IdNaoConformidade);
        var solucao = agregacao.SolucaoNaoConformidades.Find(p => p.IdNaoConformidade.Equals(idNaoConformidade));
        solucao.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }

    [Fact(DisplayName = "Remove SolucaoNaoConformidade with Success")]
    public async Task RemoveSolucaoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        await mocker.SolucaoNaoConformidade.InsertAsync(TestUtils.ObjectMother.GetSolucaoNaoConformidade(0));
        var solucaoInput = new SolucaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = true,
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = TestUtils.ObjectMother.Decimals[0],
            NovaData = TestUtils.ObjectMother.Datas[0],
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
        };
       
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Remove(idNaoConformidade, solucaoInput.Id);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        var solucao = agregacao.SolucaoNaoConformidades.Find(p => p.Id.Equals(solucaoInput.Id));
        solucao.Should().BeNull();
    }

    private SolucaoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new SolucaoNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment =TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            SolucaoNaoConformidade = ServiceProvider.GetService<IRepository<SolucaoNaoConformidade>>(),
            CurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private SolucaoNaoConformidadeService GetService(SolucaoNaoConformidadeServiceMocker mocker)
    {

        var service = new SolucaoNaoConformidadeService(mocker.NaoConformidadeRepository,
            mocker.DateTimeProvider, mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, 
            mocker.SolucaoNaoConformidade, mocker.CurrentCompany);

        return service;
    }

    public class SolucaoNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<SolucaoNaoConformidade> SolucaoNaoConformidade { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }

    }
}