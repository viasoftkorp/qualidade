using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Domain.NaoConformidades.Repositiories;

public class NaoConformidadeRepositoryTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact]
    public async Task Get()
    {
        // Arrange
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        var aggregate = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);

        var dependencies = GetDependencies();
        await dependencies.NaoConformidadeRepository.InsertAsync(aggregate.NaoConformidade);
        await dependencies.ReclamacaoClienteRepository.InsertAsync(aggregate.ReclamacaoNaoConformidade);
        await dependencies.ConclusaoNaoConformidadeRepository.InsertAsync(aggregate.ConclusaoNaoConformidade);
        await dependencies.AcaoPreventivaNaoConformidadeRepository.InsertRangeAsync(aggregate
            .AcaoPreventivaNaoConformidades);
        await dependencies.DefeitoNaoConformidadeRepository.InsertRangeAsync(aggregate.DefeitoNaoConformidades);
        await dependencies.CausaNaoConformidadeRepository.InsertRangeAsync(aggregate.CausaNaoConformidades);
        await dependencies.SolucaoNaoConformidadeRepository.InsertRangeAsync(aggregate.SolucaoNaoConformidades);
        await dependencies.ProdutoNaoConformidadeRepository.InsertRangeAsync(aggregate
            .ProdutoNaoConformidades);
        await dependencies.ServicoSolucaoNaoConformidadeRepository.InsertRangeAsync(aggregate
            .ServicoNaoConformidades);
        await dependencies.ImplementacaoEvitarReincidenciaNaoConformidadeRepository.InsertRangeAsync(aggregate
            .ImplementacoesEvitarReincidenciaNaoConformidades);
        await dependencies.CentroCustoCausaNaoConformidades.InsertRangeAsync(aggregate
            .CentroCustoCausaNaoConformidades);
        await UnitOfWork.SaveChangesAsync();
        var repository = GetRepository(dependencies);
        // Act
        var naoConformidade = await repository.Get(idNaoConformidade);
        // Assert
        naoConformidade.Should().NotBeNull();
        naoConformidade.AcaoPreventivaNaoConformidades.Should().BeEquivalentTo(aggregate.AcaoPreventivaNaoConformidades,
            op => op.ExcludingMissingMembers());
        naoConformidade.CausaNaoConformidades.Should()
            .BeEquivalentTo(aggregate.CausaNaoConformidades, op => op.ExcludingMissingMembers());
        naoConformidade.DefeitoNaoConformidades.Should()
            .BeEquivalentTo(aggregate.DefeitoNaoConformidades, op => op.ExcludingMissingMembers());
        naoConformidade.SolucaoNaoConformidades.Should()
            .BeEquivalentTo(aggregate.SolucaoNaoConformidades, op => op.ExcludingMissingMembers());
        naoConformidade.ProdutoNaoConformidades.Should().BeEquivalentTo(aggregate.ProdutoNaoConformidades,
            op => op.ExcludingMissingMembers());
        naoConformidade.ServicoNaoConformidades.Should().BeEquivalentTo(aggregate.ServicoNaoConformidades,
            op => op.ExcludingMissingMembers());
        naoConformidade.CentroCustoCausaNaoConformidades.Should().BeEquivalentTo(aggregate.CentroCustoCausaNaoConformidades,
            op => op.ExcludingMissingMembers());
        naoConformidade.ConclusaoNaoConformidade.Should()
            .BeEquivalentTo(aggregate.ConclusaoNaoConformidade, op => op.ExcludingMissingMembers());
        naoConformidade.ReclamacaoNaoConformidade.Should()
            .BeEquivalentTo(aggregate.ReclamacaoNaoConformidade, op => op.ExcludingMissingMembers());
        naoConformidade.ImplementacaoEvitarReincidencia.Should()
            .BeEquivalentTo(aggregate.ImplementacoesEvitarReincidenciaNaoConformidades, op => op.ExcludingMissingMembers());
    }

    [Fact]
    public async Task CreateTest1()
    {
        // Arrange
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        var aggregate = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);

        var dependencies = GetDependencies();
        var repository = GetRepository(dependencies);
        var input = new CreateNaoConformidadeInput
        {
            NaoConformidadeACriar = aggregate.NaoConformidade,
            ConclusaoACriar = aggregate.ConclusaoNaoConformidade,
            ReclamacaoACriar = aggregate.ReclamacaoNaoConformidade,
            AcaoPreventivaNaoConformidadesACriar = aggregate.AcaoPreventivaNaoConformidades,
            CausaNaoConformidadesACriar = aggregate.CausaNaoConformidades,
            DefeitoNaoConformidadesACriar = aggregate.DefeitoNaoConformidades,
            SolucaoNaoConformidadesACriar = aggregate.SolucaoNaoConformidades,
            ProdutoNaoConformidadesACriar = aggregate.ProdutoNaoConformidades,
            ServicoNaoConformidadesACriar = aggregate.ServicoNaoConformidades
        };
        // Act
        using (UnitOfWork.Begin())
        {
            await repository.Create(input);
            await UnitOfWork.CompleteAsync();
        }

        // Assert
        var naoConformidade =
            await dependencies.NaoConformidadeRepository.FindAsync(aggregate.NaoConformidade.Id);
        var conclusao =
            await dependencies.ConclusaoNaoConformidadeRepository.FindAsync(aggregate.ConclusaoNaoConformidade.Id);
        var reclamacao =
            await dependencies.ReclamacaoClienteRepository.FindAsync(aggregate.ReclamacaoNaoConformidade.Id);
        var acaoPreventiva = await dependencies.AcaoPreventivaNaoConformidadeRepository
            .Where(entity => aggregate.AcaoPreventivaNaoConformidades
                .Select(e => e.Id)
                .Contains(entity.Id))
            .ToListAsync();
        var causa = await dependencies.CausaNaoConformidadeRepository
            .Where(entity => aggregate.CausaNaoConformidades
                .Select(e => e.Id)
                .Contains(entity.Id))
            .ToListAsync();
        var defeito = await dependencies.DefeitoNaoConformidadeRepository
            .Where(entity => aggregate.DefeitoNaoConformidades
                .Select(e => e.Id)
                .Contains(entity.Id))
            .ToListAsync();
        var solucao = await dependencies.SolucaoNaoConformidadeRepository
            .Where(entity => aggregate.SolucaoNaoConformidades
                .Select(e => e.Id)
                .Contains(entity.Id))
            .ToListAsync();
        var produtoSolucao = await dependencies.ProdutoNaoConformidadeRepository
            .Where(entity => aggregate.ProdutoNaoConformidades
                .Select(e => e.Id)
                .Contains(entity.Id))
            .ToListAsync();
        var servicoSolucao = await dependencies.ServicoSolucaoNaoConformidadeRepository
            .Where(entity => aggregate.ServicoNaoConformidades
                .Select(e => e.Id)
                .Contains(entity.Id))
            .ToListAsync();

        naoConformidade.Should().NotBeNull();
        conclusao.Should().BeEquivalentTo(aggregate.ConclusaoNaoConformidade, op => op.ExcludingMissingMembers());
        reclamacao.Should().BeEquivalentTo(aggregate.ReclamacaoNaoConformidade, op => op.ExcludingMissingMembers());
        acaoPreventiva.Should().BeEquivalentTo(aggregate.AcaoPreventivaNaoConformidades, op => op.ExcludingMissingMembers());
        causa.Should().BeEquivalentTo(aggregate.CausaNaoConformidades, op => op.ExcludingMissingMembers());
        defeito.Should().BeEquivalentTo(aggregate.DefeitoNaoConformidades, op => op.ExcludingMissingMembers());
        solucao.Should().BeEquivalentTo(aggregate.SolucaoNaoConformidades, op => op.ExcludingMissingMembers());
        produtoSolucao.Should().BeEquivalentTo(aggregate.ProdutoNaoConformidades, op => op.ExcludingMissingMembers());
        servicoSolucao.Should().BeEquivalentTo(aggregate.ServicoNaoConformidades, op => op.ExcludingMissingMembers());
    }
    
    [Fact(DisplayName = "Se criando uma não conformidade incompleta, não deve ser preenchido código")]
    public async Task CreateTest2()
    {
        // Arrange
        var dependencies = GetDependencies();
        var repository = GetRepository(dependencies);
        
        var aggregate = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var naoConformidade = aggregate.NaoConformidade;
        naoConformidade.Incompleta = true;
        naoConformidade.Codigo = null;
        
        var input = new CreateNaoConformidadeInput
        {
            NaoConformidadeACriar = naoConformidade,
            ConclusaoACriar = aggregate.ConclusaoNaoConformidade,
            ReclamacaoACriar = aggregate.ReclamacaoNaoConformidade,
            AcaoPreventivaNaoConformidadesACriar = aggregate.AcaoPreventivaNaoConformidades,
            CausaNaoConformidadesACriar = aggregate.CausaNaoConformidades,
            DefeitoNaoConformidadesACriar = aggregate.DefeitoNaoConformidades,
            SolucaoNaoConformidadesACriar = aggregate.SolucaoNaoConformidades,
            ProdutoNaoConformidadesACriar = aggregate.ProdutoNaoConformidades,
            ServicoNaoConformidadesACriar = aggregate.ServicoNaoConformidades
        };
        
        var expectedResult = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = null,
            Origem = OrigemNaoConformidade.Cliente,
            Status = StatusNaoConformidade.Aberto,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = TestUtils.ObjectMother.Strings[0],
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            CreatorId = TestUtils.ObjectMother.Guids[0],
            Incompleta = true,
            DataCriacao = TestUtils.ObjectMother.Datas[0],
            NumeroNotaFiscal = TestUtils.ObjectMother.Ints[0].ToString(),
            NumeroLote = TestUtils.ObjectMother.Ints[0].ToString(),
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
            NumeroPedido = TestUtils.ObjectMother.Strings[0],
            NumeroOdfFaturamento = TestUtils.ObjectMother.Ints[0],
            IdProdutoFaturamento = TestUtils.ObjectMother.Guids[0]
        };
       
        // Act
        using (UnitOfWork.Begin())
        {
            await repository.Create(input);
            await UnitOfWork.CompleteAsync();
        }

        // Assert
        var naoConformidadeResult =
            await dependencies.NaoConformidadeRepository.FindAsync(aggregate.NaoConformidade.Id);
        
        naoConformidadeResult.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    
    [Fact(DisplayName = "Se criando uma não conformidade completa, deve ser buscado o código e o mesmo deve ser salvo")]
    public async Task CreateTest3()
    {
        // Arrange
        var dependencies = GetDependencies();
        var repository = GetRepository(dependencies);
        
        var aggregate = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var naoConformidade = aggregate.NaoConformidade;
        naoConformidade.Incompleta = false;
        naoConformidade.Codigo = null;

        dependencies.GeracaoCodigoService.GetCodigoNaoConformidade().Returns(25);
        var input = new CreateNaoConformidadeInput
        {
            NaoConformidadeACriar = naoConformidade,
            ConclusaoACriar = aggregate.ConclusaoNaoConformidade,
            ReclamacaoACriar = aggregate.ReclamacaoNaoConformidade,
            AcaoPreventivaNaoConformidadesACriar = aggregate.AcaoPreventivaNaoConformidades,
            CausaNaoConformidadesACriar = aggregate.CausaNaoConformidades,
            DefeitoNaoConformidadesACriar = aggregate.DefeitoNaoConformidades,
            SolucaoNaoConformidadesACriar = aggregate.SolucaoNaoConformidades,
            ProdutoNaoConformidadesACriar = aggregate.ProdutoNaoConformidades,
            ServicoNaoConformidadesACriar = aggregate.ServicoNaoConformidades
        };
        
        var expectedResult = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 25,
            Origem = OrigemNaoConformidade.Cliente,
            Status = StatusNaoConformidade.Aberto,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = TestUtils.ObjectMother.Guids[0],
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = TestUtils.ObjectMother.Strings[0],
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = TestUtils.ObjectMother.Strings[0],
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[0],
            CompanyId = TestUtils.ObjectMother.Guids[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            CreatorId = TestUtils.ObjectMother.Guids[0],
            Incompleta = false,
            DataCriacao = TestUtils.ObjectMother.Datas[0],
            NumeroNotaFiscal = TestUtils.ObjectMother.Ints[0].ToString(),
            NumeroLote = TestUtils.ObjectMother.Ints[0].ToString(),
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
            NumeroPedido = TestUtils.ObjectMother.Strings[0],
            NumeroOdfFaturamento = TestUtils.ObjectMother.Ints[0],
            IdProdutoFaturamento = TestUtils.ObjectMother.Guids[0]
        };
        
        // Act
        using (UnitOfWork.Begin())
        {
            await repository.Create(input);
            await UnitOfWork.CompleteAsync();
        }

        // Assert
        var naoConformidadeResult =
            await dependencies.NaoConformidadeRepository.FindAsync(aggregate.NaoConformidade.Id);
        
        naoConformidadeResult.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }

    private NaoConformidadeRepository GetRepository(RepositoryDependenciesMock dependencies)
    {
        return new NaoConformidadeRepository(dependencies.NaoConformidadeRepository,
            dependencies.AcaoPreventivaNaoConformidadeRepository, dependencies.CausaNaoConformidadeRepository,
            dependencies.DefeitoNaoConformidadeRepository, dependencies.SolucaoNaoConformidadeRepository,
            dependencies.ProdutoNaoConformidadeRepository, dependencies.ServicoSolucaoNaoConformidadeRepository,
            dependencies.CentroCustoCausaNaoConformidades,
            dependencies.ConclusaoNaoConformidadeRepository, dependencies.ReclamacaoClienteRepository, 
            dependencies.ImplementacaoEvitarReincidenciaNaoConformidadeRepository, dependencies.GeracaoCodigoService,
            dependencies.OrdemRetrabalhoNaoConformidades
        );
    }

    private RepositoryDependenciesMock GetDependencies()
    {
        var mocker = new RepositoryDependenciesMock
        {
            NaoConformidadeRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>(),
            AcaoPreventivaNaoConformidadeRepository =
                ServiceProvider.GetService<IRepository<AcaoPreventivaNaoConformidade>>(),
            CausaNaoConformidadeRepository = ServiceProvider.GetService<IRepository<CausaNaoConformidade>>(),
            DefeitoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<DefeitoNaoConformidade>>(),
            SolucaoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<SolucaoNaoConformidade>>(),
            ProdutoNaoConformidadeRepository =
                ServiceProvider.GetService<IRepository<ProdutoNaoConformidade>>(),
            ServicoSolucaoNaoConformidadeRepository =
                ServiceProvider.GetService<IRepository<ServicoNaoConformidade>>(),
            CentroCustoCausaNaoConformidades = 
                ServiceProvider.GetService<IRepository<CentroCustoCausaNaoConformidade>>(),
            ReclamacaoClienteRepository = ServiceProvider.GetService<IRepository<ReclamacaoNaoConformidade>>(),
            ConclusaoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<ConclusaoNaoConformidade>>(),
            ImplementacaoEvitarReincidenciaNaoConformidadeRepository = ServiceProvider.GetService<IRepository<ImplementacaoEvitarReincidenciaNaoConformidade>>(),
            GeracaoCodigoService = Substitute.For<IGeracaoCodigoService>(),
            OrdemRetrabalhoNaoConformidades = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>()
        };
        return mocker;
    }
}

public class RepositoryDependenciesMock
{
    public IRepository<NaoConformidade> NaoConformidadeRepository { get; set; }
    public IRepository<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidadeRepository { get; set; }
    public IRepository<CausaNaoConformidade> CausaNaoConformidadeRepository { get; set; }
    public IRepository<DefeitoNaoConformidade> DefeitoNaoConformidadeRepository { get; set; }
    public IRepository<SolucaoNaoConformidade> SolucaoNaoConformidadeRepository { get; set; }
    public IRepository<ProdutoNaoConformidade> ProdutoNaoConformidadeRepository { get; set; }
    public IRepository<ServicoNaoConformidade> ServicoSolucaoNaoConformidadeRepository { get; set; }
    public IRepository<ReclamacaoNaoConformidade> ReclamacaoClienteRepository { get; set; }
    public IRepository<ConclusaoNaoConformidade> ConclusaoNaoConformidadeRepository { get; set; }
    public IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidenciaNaoConformidadeRepository { get; set; }
    public IRepository<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidades { get; set; }
    public IGeracaoCodigoService GeracaoCodigoService { get; set; }
    public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidades { get; set; }
}
