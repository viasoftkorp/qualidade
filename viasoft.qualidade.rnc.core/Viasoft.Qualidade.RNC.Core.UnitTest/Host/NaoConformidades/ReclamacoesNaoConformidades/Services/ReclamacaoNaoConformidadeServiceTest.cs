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
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

public class ReclamacaoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Criar ReclamacaoNaoConformidade with Success")]
    public async Task CriarReclamacaoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var reclamacaoInput = new ReclamacaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Procedentes = TestUtils.ObjectMother.Ints[0],
            Improcedentes = TestUtils.ObjectMother.Ints[0],
            QuantidadeLote = TestUtils.ObjectMother.Ints[0],
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosAprovados = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosConcessao = TestUtils.ObjectMother.Ints[0],
            Retrabalho = TestUtils.ObjectMother.Ints[0],
            Rejeitado = TestUtils.ObjectMother.Ints[0],
            RetrabalhoComOnus = true,
            RetrabalhoSemOnus = true,
            DevolucaoFornecedor = true,
            Recodificar = true,
            Sucata = true,
            Observacao = TestUtils.ObjectMother.Strings[0]
        };
        var expectedResult = new ReclamacaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Procedentes = TestUtils.ObjectMother.Ints[0],
            Improcedentes = TestUtils.ObjectMother.Ints[0],
            QuantidadeLote = TestUtils.ObjectMother.Ints[0],
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosAprovados = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosConcessao = TestUtils.ObjectMother.Ints[0],
            Retrabalho = TestUtils.ObjectMother.Ints[0],
            Rejeitado = TestUtils.ObjectMother.Ints[0],
            RetrabalhoComOnus = true,
            RetrabalhoSemOnus = true,
            DevolucaoFornecedor = true,
            Recodificar = true,
            Sucata = true,
            Observacao = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0]
        };

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Insert(idNaoConformidade, reclamacaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(reclamacaoInput.IdNaoConformidade);
        var reclamacao = agregacao.ReclamacaoNaoConformidadeInserir;
        reclamacao.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Update ReclamacaoNaoConformidade with Success")]
    public async Task UpdateReclamacaoNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        var reclamacaoInput = new ReclamacaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Procedentes = TestUtils.ObjectMother.Ints[0],
            Improcedentes = TestUtils.ObjectMother.Ints[0],
            QuantidadeLote = TestUtils.ObjectMother.Ints[0],
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosAprovados = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosConcessao = TestUtils.ObjectMother.Ints[0],
            Retrabalho = TestUtils.ObjectMother.Ints[0],
            Rejeitado = TestUtils.ObjectMother.Ints[0],
            RetrabalhoComOnus = true,
            RetrabalhoSemOnus = true,
            DevolucaoFornecedor = true,
            Recodificar = true,
            Sucata = true,
            Observacao = TestUtils.ObjectMother.Strings[0]
        };
        var expectedResult = new ReclamacaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Procedentes = TestUtils.ObjectMother.Ints[0],
            Improcedentes = TestUtils.ObjectMother.Ints[0],
            QuantidadeLote = TestUtils.ObjectMother.Ints[0],
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosAprovados = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosConcessao = TestUtils.ObjectMother.Ints[0],
            Retrabalho = TestUtils.ObjectMother.Ints[0],
            Rejeitado = TestUtils.ObjectMother.Ints[0],
            RetrabalhoComOnus = true,
            RetrabalhoSemOnus = true,
            DevolucaoFornecedor = true,
            Recodificar = true,
            Sucata = true,
            Observacao = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0]
        };

        await service.Insert(idNaoConformidade, reclamacaoInput);

        await UnitOfWork.SaveChangesAsync();

        //Act
        await service.Update(idNaoConformidade, reclamacaoInput);

        //Assert
        var agregacao = await mocker.NaoConformidadeRepository.Get(reclamacaoInput.IdNaoConformidade);
        var reclamacao = agregacao.ReclamacaoNaoConformidade;
        reclamacao.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "GetReclamacaoNaoConformidade with Success")]
    public async Task GetReclamacaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetReclamacaoNaoConformidade(0);
        
        await mocker.ReclamacaoNaoConformidade.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(input.IdNaoConformidade);

        //Assert
        var reclamacao = await mocker.ReclamacaoNaoConformidade.FindAsync(TestUtils.ObjectMother.Guids[0]);
        reclamacao.Should().BeEquivalentTo(output, options => options.Excluding(e => e.CompanyId));
    }
    private ReclamacaoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new ReclamacaoNaoConformidadeServiceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment =TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            ReclamacaoNaoConformidade = ServiceProvider.GetService<IRepository<ReclamacaoNaoConformidade>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private ReclamacaoNaoConformidadeService GetService(ReclamacaoNaoConformidadeServiceMocker mocker)
    {

        var service = new ReclamacaoNaoConformidadeService(mocker.NaoConformidadeRepository, mocker.DateTimeProvider,
            mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, mocker.ReclamacaoNaoConformidade,
            mocker.FakeCurrentCompany);

        return service;
    }

    public class ReclamacaoNaoConformidadeServiceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<ReclamacaoNaoConformidade> ReclamacaoNaoConformidade { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}