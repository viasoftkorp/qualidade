/*using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

public class ConclusaoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "ConcluirNaoConformidade with Success")]
    public async Task ConcluirNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var conclusaoInput = new ConclusaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NovaReuniao = false,
            DataReuniao = TestUtils.ObjectMother.Datas[0],
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Evidencia = TestUtils.ObjectMother.Strings[0],
            Eficaz = false,
            CicloDeTempo = TestUtils.ObjectMother.Ints[0],
            IdNovoRelatorio = TestUtils.ObjectMother.Guids[0],
        };
        var expectedResult = new ConclusaoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NovaReuniao = false,
            DataReuniao = TestUtils.ObjectMother.Datas[0],
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Evidencia = TestUtils.ObjectMother.Strings[0],
            Eficaz = false,
            CicloDeTempo = TestUtils.ObjectMother.Ints[0],
            IdNovoRelatorio = TestUtils.ObjectMother.Guids[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.ConcluirNaoConformidade(idNaoConformidade, conclusaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(conclusaoInput.IdNaoConformidade);
        var conclusao = agregacao.ConclusaoNaoConformidade;
        conclusao.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
    }
    
    [Fact(DisplayName = "Se status não conformidade for Fechado, deve estornar não conformidade")]
    public async Task EstornarTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.AgregacaoNaoConformidades[0];
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        agregacaoCriada.NaoConformidade.Status = StatusNaoConformidade.Fechado;
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Estornar(idNaoConformidade);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(idNaoConformidade);
        agregacao.ConclusaoNaoConformidade.Should().BeNull();
    }
    
    [Fact(DisplayName = "GetConclusaoNaoConformidade with Success")]
    public async Task GetConclusaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetConclusaoNaoConformidade(0);
        
        await mocker.ConclusaoNaoConformidade.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade);

        //Assert
        var conclusao = await mocker.ConclusaoNaoConformidade.FindAsync(TestUtils.ObjectMother.Guids[0]);
        conclusao.Should().BeEquivalentTo(output, options => options.Excluding(e => e.CompanyId));
    }
    private ConclusaoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new ConclusaoNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment =TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            ConclusaoNaoConformidade = ServiceProvider.GetService<IRepository<ConclusaoNaoConformidade>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private ConclusaoNaoConformidadeService GetService(ConclusaoNaoConformidadeServiceMocker mocker)
    {

        var service = new ConclusaoNaoConformidadeService(mocker.NaoConformidadeRepository, mocker.DateTimeProvider,
            mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, mocker.ConclusaoNaoConformidade,
            mocker.FakeCurrentCompany);

        return service;
    }

    public class ConclusaoNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<ConclusaoNaoConformidade> ConclusaoNaoConformidade { get; set; }
        
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}*/