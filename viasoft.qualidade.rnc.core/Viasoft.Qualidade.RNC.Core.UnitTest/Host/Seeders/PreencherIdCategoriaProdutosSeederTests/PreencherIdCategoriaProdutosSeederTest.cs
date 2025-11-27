using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdCategoriaProdutosSeeders;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdCategoriaProdutosSeeders.Contracts.Commands;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Seeders.PreencherIdCategoriaProdutosSeederTests;

public class PreencherIdCategoriaProdutosSeederTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se houver produtos, deve buscar idCategoria de cada um deles e atualiza-los com este id")]
    public async Task SeedPreencherIdCategoriaProdutosHandleTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetSeederHandler(mocker);
        await mocker.SeederManagers.InsertAsync(new SeederManager
        {
            Id = TestUtils.ObjectMother.Guids[0]
        });
        var produtos = new List<Produto>
        {
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdCategoria = null,
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
                TenantId = TestUtils.ObjectMother.Guids[0],
                EnvironmentId = TestUtils.ObjectMother.Guids[0],
                CreationTime = TestUtils.ObjectMother.Datas[0],
                CreatorId = TestUtils.ObjectMother.Guids[0],
                LastModificationTime = null,
                LastModifierId = null
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[1],
                IdCategoria = null,
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[1],
                TenantId = TestUtils.ObjectMother.Guids[1],
                EnvironmentId = TestUtils.ObjectMother.Guids[1],
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[2],
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                Descricao = TestUtils.ObjectMother.Strings[2],
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[2],
                TenantId = TestUtils.ObjectMother.Guids[2],
                EnvironmentId = TestUtils.ObjectMother.Guids[2],
                CreationTime = TestUtils.ObjectMother.Datas[2],
                CreatorId = TestUtils.ObjectMother.Guids[2],
                LastModificationTime = null,
                LastModifierId = null
            }
        };
        await mocker.ProdutosRepository.InsertRangeAsync(produtos, true);
        var produtosIds = await mocker.ProdutosRepository.Where(e => !e.IsDeleted).Select(e => e.Id).ToListAsync();
        mocker.ProdutosProxyService.GetAllByIdsPaginando(Arg.Is<List<Guid>>(e=> e.SequenceEqual(produtosIds))).Returns(
            new List<ProdutoOutput>
            {
                 new ProdutoOutput
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdCategoria = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdUnidade = TestUtils.ObjectMother.Guids[0]
            },
            new ProdutoOutput
            {
                Id = TestUtils.ObjectMother.Guids[1],
                IdCategoria = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdUnidade = TestUtils.ObjectMother.Guids[1]
            },
            new ProdutoOutput
            {
                Id = TestUtils.ObjectMother.Guids[2],
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                Descricao = TestUtils.ObjectMother.Strings[2],
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdUnidade = TestUtils.ObjectMother.Guids[2]
            }
            });
        
        var expectedResult = new List<Produto>
        {
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdCategoria = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
                TenantId = TestUtils.ObjectMother.Guids[0],
                EnvironmentId = TestUtils.ObjectMother.Guids[0],
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[1],
                IdCategoria = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[1],
                TenantId = TestUtils.ObjectMother.Guids[1],
                EnvironmentId = TestUtils.ObjectMother.Guids[1],
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[2],
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                Descricao = TestUtils.ObjectMother.Strings[2],
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[2],
                TenantId = TestUtils.ObjectMother.Guids[2],
                EnvironmentId = TestUtils.ObjectMother.Guids[2],
            }
        };
        //Act
        await handler.Handle(new SeedPreencherIdCategoriaProdutosMessage());
        //Assert
        var produtosResult = await mocker.ProdutosRepository.ToListAsync();
        produtosResult.Should().BeEquivalentTo(expectedResult, options => ExcludeFullAuditedEntityFields(options));
    }
    [Fact(DisplayName = "Se tudo processado, deve modificar PreencherIdCategoriaProdutosSeederFinalizado para true ")]
    public async Task SeedPreencherIdCategoriaProdutosHandleTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetSeederHandler(mocker);
        await mocker.SeederManagers.InsertAsync(new SeederManager
        {
            Id = TestUtils.ObjectMother.Guids[0],
            PreencherIdCategoriaProdutosSeederFinalizado = false
        });
        var produtos = new List<Produto>
        {
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdCategoria = null,
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
                TenantId = TestUtils.ObjectMother.Guids[0],
                EnvironmentId = TestUtils.ObjectMother.Guids[0],
                CreationTime = TestUtils.ObjectMother.Datas[0],
                CreatorId = TestUtils.ObjectMother.Guids[0],
                LastModificationTime = null,
                LastModifierId = null
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[1],
                IdCategoria = null,
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[1],
                TenantId = TestUtils.ObjectMother.Guids[1],
                EnvironmentId = TestUtils.ObjectMother.Guids[1],
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[2],
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                Descricao = TestUtils.ObjectMother.Strings[2],
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[2],
                TenantId = TestUtils.ObjectMother.Guids[2],
                EnvironmentId = TestUtils.ObjectMother.Guids[2],
                CreationTime = TestUtils.ObjectMother.Datas[2],
                CreatorId = TestUtils.ObjectMother.Guids[2],
                LastModificationTime = null,
                LastModifierId = null
            }
        };
        await mocker.ProdutosRepository.InsertRangeAsync(produtos, true);
        var produtosIds = await mocker.ProdutosRepository.Where(e => !e.IsDeleted).Select(e => e.Id).ToListAsync();
        mocker.ProdutosProxyService.GetAllByIdsPaginando(Arg.Is<List<Guid>>(e=> e.SequenceEqual(produtosIds))).Returns(
            new List<ProdutoOutput>
            {
                 new ProdutoOutput
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdCategoria = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdUnidade = TestUtils.ObjectMother.Guids[0]
            },
            new ProdutoOutput
            {
                Id = TestUtils.ObjectMother.Guids[1],
                IdCategoria = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdUnidade = TestUtils.ObjectMother.Guids[1]
            },
            new ProdutoOutput
            {
                Id = TestUtils.ObjectMother.Guids[2],
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                Descricao = TestUtils.ObjectMother.Strings[2],
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdUnidade = TestUtils.ObjectMother.Guids[2]
            }
            });
        
        var expectedResult = new SeederManager
        {
           Id = TestUtils.ObjectMother.Guids[0],
           PreencherIdCategoriaProdutosSeederFinalizado = true
        };
        //Act
        await handler.Handle(new SeedPreencherIdCategoriaProdutosMessage());
        //Assert
        var seederManager = await mocker.SeederManagers.FirstAsync();
        seederManager.Should().BeEquivalentTo(expectedResult, options => options.ExcludingMissingMembers());
    }
    private class Mocker
    {
        public IRepository<Produto> ProdutosRepository { get; set; }
        public IProdutosProxyService ProdutosProxyService { get; set; }
        public IRepository<SeederManager> SeederManagers { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            ProdutosRepository = ServiceProvider.GetService<IRepository<Produto>>(),
            ProdutosProxyService = Substitute.For<IProdutosProxyService>(),
            SeederManagers = ServiceProvider.GetService<IRepository<SeederManager>>()
        };

        return mocker;
    }

    private PreencherIdCategoriaProdutosHandler GetSeederHandler(Mocker mocker)
    {
        var service = new PreencherIdCategoriaProdutosHandler(mocker.ProdutosRepository, mocker.ProdutosProxyService, mocker.SeederManagers, UnitOfWork);
        return service;
    }
    private static EquivalencyAssertionOptions<T> ExcludeFullAuditedEntityFields<T>(EquivalencyAssertionOptions<T> options) where T : FullAuditedEntity
    {
        return options
            .Excluding(e => e.CreationTime)
            .Excluding(e => e.DeletionTime)
            .Excluding(e => e.LastModificationTime)
            .Excluding(e => e.LastModifierId)
            .Excluding(e => e.CreatorId);
    }
}