using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.PedidoVendas;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.OrdemRetrabalhos.Services;

public class GerarOrdemRetrabalhoAclServiceTests : OrdemRetrabalhoAclServiceTest
{
    [Fact(DisplayName = "Se numero odf for igual a 0, pedido deve ser igual à \"991\"")]
    public async Task GerarOrdemRetrabalhoTest1()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        input.NumeroPedido = "0";
        
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);

        var idsCategorias = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0]
        };
        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>
            {
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        var idsRecursos = agregacao.ServicoNaoConformidades.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });
        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var ordemProducao = new OrdemProducaoOutput()
        {
            DataEntrega = TestUtils.ObjectMother.Datas[0],
            NumeroOdf = TestUtils.ObjectMother.Ints[0]
        };

        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            ordemProducao);
        var localDestino = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);

        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = "991",
            OdfOrigem = ordemProducao.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>(),
            Materias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>()
        };
        
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se numero pedido for igual a 991, pedido deve ser igual à \"991\"")]
    public async Task GerarOrdemRetrabalhoTest2()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        input.Quantidade = 1;
        input.NumeroOdfOrigem = 1;
        input.NumeroPedido = "991";
        
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        
        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);

        var idsCategorias = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0]
        };
        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>
            {
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        var idsRecursos = agregacao.ServicoNaoConformidades.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var itemPedidoVenda = GetOrdemProducaoOutput(0);
        itemPedidoVenda.NumeroPedido = "";
        
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            itemPedidoVenda);

        var localDestino = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);

        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = itemPedidoVenda.DataEntrega.AddDateMask(),
            Pedido = "991",
            OdfOrigem = itemPedidoVenda.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>(),
            Materias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>()
        };
        
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se numero pedido for diferente de 991, pedido deve ser igual ao numero pedido da odf")]
    public async Task GerarOrdemRetrabalhoTest3()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        input.NumeroPedido = "900";
        
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        var ordemProducaoOutput = GetOrdemProducaoOutput(0);
        
        mocker.OrdemProducaoProvider.GetByNumeroOdf(0, true).Returns(ordemProducaoOutput);

        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);

        var idsCategorias = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0]
        };
        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>
            {
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        var idsRecursos = agregacao.ServicoNaoConformidades.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var ordemProducao = GetOrdemProducaoOutput(0);
        
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            ordemProducao);
        var localDestino = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);
        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = "900",
            OdfOrigem = ordemProducao.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>(),
            Materias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>()
        };

        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName =
        "Se houver produtos, a sequencia dos materiais deve ser a sequencia dos itens agrupando produtos pela operação engenharia")]
    public async Task GerarOrdemRetrabalhoTest4()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        var materiais = new List<GerarOrdemRetrabalhoMaterialInput>
        {
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Quantidade = 1,
                IdProduto = TestUtils.ObjectMother.Guids[0],
            },
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Quantidade = 2,
                IdProduto = TestUtils.ObjectMother.Guids[1],
            },
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "2",
                Quantidade = 3,
                IdProduto = TestUtils.ObjectMother.Guids[2],
            },
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "3",
                Quantidade = 4,
                IdProduto = TestUtils.ObjectMother.Guids[3],
            }
        };

        input.MateriaisInput.AddRange(materiais);
        input.MaquinasInput.Add(new GerarOrdemRetrabalhoMaquinaInput
        {
            Horas = TestUtils.ObjectMother.Ints[0],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            Operacao = TestUtils.ObjectMother.Strings[0]
        });

        await mocker.ProdutosRepository.InsertRangeAsync(new List<Produto>
        {
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[0],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
                Id = TestUtils.ObjectMother.Guids[0]
            },
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[1],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[1],
                Id = TestUtils.ObjectMother.Guids[1]
            },
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[2],
                Id = TestUtils.ObjectMother.Guids[2]
            },
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[3].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[3],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[3],
                Id = TestUtils.ObjectMother.Guids[3]
            },
        }, true);

        var idsCategorias = await mocker.ProdutosRepository.Where(e => e.IdCategoria.HasValue)
            .Select(e => e.IdCategoria.Value).ToListAsync();

        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>
            {
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                },
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[1],
                    Id = TestUtils.ObjectMother.Guids[1]
                },
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[2],
                    Id = TestUtils.ObjectMother.Guids[2]
                },
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[3].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[3],
                    Id = TestUtils.ObjectMother.Guids[3]
                }
            });

        var idsRecursos = agregacao.ServicoNaoConformidades.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);
        var localDestino = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);

        var ordemProducao = GetOrdemProducaoOutput(0);

        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            ordemProducao);
        
        var expectedMaquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>
        {
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                QuantidadeHoras = TestUtils.ObjectMother.Ints[0],
                Operacao = TestUtils.ObjectMother.Strings[0],
                Sequencia = "1",
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            }
        };
        
        var expectedMaterias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>
        {
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Sequencia = "1",
                CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[0].ToString(),
                Quantidade = 1,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Sequencia = "2",
                CodigoProduto = TestUtils.ObjectMother.Ints[1].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[1].ToString(),
                Quantidade = 2,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "2",
                Sequencia = "1",
                CodigoProduto = TestUtils.ObjectMother.Ints[2].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[2].ToString(),
                Quantidade = 3,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "3",
                Sequencia = "1",
                CodigoProduto = TestUtils.ObjectMother.Ints[3].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[3].ToString(),
                Quantidade = 4,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            }
        };

        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, false),
            OdfOrigem = ordemProducao.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = expectedMaquinas,
            Materias = expectedMaterias
        };
        
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName =
        "Se houver serviços e houver materiais, a sequencia da maquina deve ser a proxima sequencia depois do ultimo material, agrupado pela operação engenharia")]
    public async Task GerarOrdemRetrabalhoTest5()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        
        var materiais = new List<GerarOrdemRetrabalhoMaterialInput>
        {
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Quantidade = 1,
                IdProduto = TestUtils.ObjectMother.Guids[0],
            },
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Quantidade = 2,
                IdProduto = TestUtils.ObjectMother.Guids[1],
            },
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "2",
                Quantidade = 3,
                IdProduto = TestUtils.ObjectMother.Guids[2],
            },
            new GerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "3",
                Quantidade = 4,
                IdProduto = TestUtils.ObjectMother.Guids[3],
            }
        };

        var maquinas = new List<GerarOrdemRetrabalhoMaquinaInput>
        {
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "1",
                Horas = TestUtils.ObjectMother.Ints[0],
                Detalhamento = TestUtils.ObjectMother.Strings[0],
                IdRecurso = TestUtils.ObjectMother.Guids[0],
            },
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "2",
                Horas = TestUtils.ObjectMother.Ints[1],
                Detalhamento = TestUtils.ObjectMother.Strings[1],
                IdRecurso = TestUtils.ObjectMother.Guids[1],
            },
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "3",
                Horas = TestUtils.ObjectMother.Ints[2],
                Detalhamento = TestUtils.ObjectMother.Strings[2],
                IdRecurso = TestUtils.ObjectMother.Guids[2],
            }
        };
        input.MateriaisInput.AddRange(materiais);

        input.MaquinasInput.AddRange(maquinas);

        await mocker.ProdutosRepository.InsertRangeAsync(new List<Produto>
        {
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[0],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
                Id = TestUtils.ObjectMother.Guids[0]
            },
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[1],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[1],
                Id = TestUtils.ObjectMother.Guids[1]
            },
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[2],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[2],
                Id = TestUtils.ObjectMother.Guids[2]
            },
            new Produto
            {
                Codigo = TestUtils.ObjectMother.Ints[3].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[3],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[3],
                Id = TestUtils.ObjectMother.Guids[3]
            },
        }, true);

        var idsCategorias = await mocker.ProdutosRepository.Where(e => e.IdCategoria.HasValue)
            .Select(e => e.IdCategoria.Value).ToListAsync();

        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>
            {
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                },
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[1],
                    Id = TestUtils.ObjectMother.Guids[1]
                },
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[2],
                    Id = TestUtils.ObjectMother.Guids[2]
                },
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[3].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[3],
                    Id = TestUtils.ObjectMother.Guids[3]
                }
            });

        var idsRecursos = input.MaquinasInput.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0],
                    LegacyId = TestUtils.ObjectMother.Ints[0]
                },
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[1],
                    Id = TestUtils.ObjectMother.Guids[1],
                    LegacyId = TestUtils.ObjectMother.Ints[1]
                },
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[2],
                    Id = TestUtils.ObjectMother.Guids[2],
                    LegacyId = TestUtils.ObjectMother.Ints[2]
                }
            });

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var localDestino = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);
        var ordemProducao = GetOrdemProducaoOutput(0);

        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            ordemProducao);
        
        var expectedMaquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>
        {
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "1",
                Sequencia = "3",
                DescricaoOperacao = input.MaquinasInput[0].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[0],
                QuantidadeHoras = input.MaquinasInput[0].Horas,
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "2",
                Sequencia = "2",
                DescricaoOperacao = input.MaquinasInput[1].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[1],
                QuantidadeHoras = input.MaquinasInput[1].Horas,
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "3",
                Sequencia = "2",
                DescricaoOperacao = input.MaquinasInput[2].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[2],
                QuantidadeHoras = input.MaquinasInput[2].Horas,
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            }
        };
        
        var expectedMaterias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>
        {
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Sequencia = "1",
                CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[0].ToString(),
                Quantidade = 1,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "1",
                Sequencia = "2",
                CodigoProduto = TestUtils.ObjectMother.Ints[1].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[1].ToString(),
                Quantidade = 2,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "2",
                Sequencia = "1",
                CodigoProduto = TestUtils.ObjectMother.Ints[2].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[2].ToString(),
                Quantidade = 3,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaterialInput
            {
                Operacao = "3",
                Sequencia = "1",
                CodigoProduto = TestUtils.ObjectMother.Ints[3].ToString(),
                Categoria = TestUtils.ObjectMother.Ints[3].ToString(),
                Quantidade = 4,
                Revisao = "",
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                Expedicao = "N",
                NaoGerarCusto = "N",
                Reaproveitado = "N",
                AjustarQtdApontamento = "N"
            }
        };
        
        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, false),
            OdfOrigem = ordemProducao.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = expectedMaquinas,
            Materias = expectedMaterias
        };
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName =
        "Se houver serviços e não houver materiais, a sequencia da maquina deve ser 1")]
    public async Task GerarOrdemRetrabalhoTest6()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = GetGerarOrdemRetrabalhoInput(0);
        
        var maquinas = new List<GerarOrdemRetrabalhoMaquinaInput>
        {
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "1",
                Horas = TestUtils.ObjectMother.Ints[0],
                Detalhamento = TestUtils.ObjectMother.Strings[0],
                IdRecurso = TestUtils.ObjectMother.Guids[0],
            },
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "2",
                Horas = TestUtils.ObjectMother.Ints[1],
                Detalhamento = TestUtils.ObjectMother.Strings[1],
                IdRecurso = TestUtils.ObjectMother.Guids[1],
            },
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "3",
                Horas = TestUtils.ObjectMother.Ints[2],
                Detalhamento = TestUtils.ObjectMother.Strings[2],
                IdRecurso = TestUtils.ObjectMother.Guids[2],
            }
        };

        input.MaquinasInput.AddRange(maquinas);

        var idsCategorias = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3]
        };
        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>());

        var idsRecursos = input.MaquinasInput.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0],
                    LegacyId = TestUtils.ObjectMother.Ints[0]
                },
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[1],
                    Id = TestUtils.ObjectMother.Guids[1],
                    LegacyId = TestUtils.ObjectMother.Ints[1]
                },
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[2],
                    Id = TestUtils.ObjectMother.Guids[2],
                    LegacyId = TestUtils.ObjectMother.Ints[2]
                }
            });
        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var localDestino = new LocalOutput
        {
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Id = TestUtils.ObjectMother.Guids[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);

        var ordemProducao = GetOrdemProducaoOutput(0);

        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            ordemProducao);
        
        var expectedMaquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>
        {
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "1",
                Sequencia = "1",
                DescricaoOperacao = input.MaquinasInput[0].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[0],
                QuantidadeHoras = input.MaquinasInput[0].Horas,
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "2",
                Sequencia = "1",
                DescricaoOperacao = input.MaquinasInput[1].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[1],
                QuantidadeHoras = input.MaquinasInput[1].Horas,
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "3",
                Sequencia = "1",
                DescricaoOperacao = input.MaquinasInput[2].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[2],
                QuantidadeHoras = input.MaquinasInput[2].Horas,
                ApontarMaquina = true,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            }
        };
        
        
        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, false),
            OdfOrigem = ordemProducao.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = expectedMaquinas,
            Materias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>()
        };
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName =
        "Se utilizarReservaDePedidoNaLocalizacaoDeEstoque, numero odf de origem deve ser o numero odf de destino da odf de origem")]
    public async Task GerarOrdemRetrabalhoTest8()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        var ordemProducao = new OrdemProducaoOutput()
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            DataEntrega = TestUtils.ObjectMother.Datas[0],
            NumeroOdfDestino = TestUtils.ObjectMother.Ints[1]
        };
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false)
            .Returns(ordemProducao);
        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);

        var idsCategorias = await mocker.ProdutosRepository.Where(e => e.IdCategoria.HasValue)
            .Select(e => e.IdCategoria.Value).ToListAsync();

        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>
            {
                new CategoriaProdutoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        var idsRecursos = agregacao.ServicoNaoConformidades.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0]
                }
            });

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var localDestino = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);

        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(true);

        var expectedResult = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, false),
            OdfOrigem = ordemProducao.NumeroOdfDestino.Value,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>(),
            Materias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>()
        };
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Se horas for 1 e minutos for 30, quantidadeHoras deve ser 1.5")]
    public async Task GerarOrdemRetrabalhoTest9()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = GetGerarOrdemRetrabalhoInput(0);
        
        var maquinas = new List<GerarOrdemRetrabalhoMaquinaInput>
        {
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "1",
                Horas = 1,
                Minutos = 30,
                Detalhamento = TestUtils.ObjectMother.Strings[0],
                IdRecurso = TestUtils.ObjectMother.Guids[0],
            },
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "2",
                Horas = 1,
                Minutos = 60,
                Detalhamento = TestUtils.ObjectMother.Strings[1],
                IdRecurso = TestUtils.ObjectMother.Guids[1],
            },
            new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "3",
                Horas = 10,
                Minutos = 150,
                Detalhamento = TestUtils.ObjectMother.Strings[2],
                IdRecurso = TestUtils.ObjectMother.Guids[2],
            }
        };

        input.MaquinasInput.AddRange(maquinas);

        var idsCategorias = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0],
            TestUtils.ObjectMother.Guids[1],
            TestUtils.ObjectMother.Guids[2],
            TestUtils.ObjectMother.Guids[3]
        };
        mocker.CategoriaProdutoProvider
            .GetAllCategoriasPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsCategorias)))
            .Returns(new List<CategoriaProdutoOutput>());

        var idsRecursos = input.MaquinasInput.ConvertAll(e => e.IdRecurso);

        mocker.RecursosProxyService
            .GetAllByIdsPaginando(Arg.Is<List<Guid>>(e => e.SequenceEqual(idsRecursos)))
            .Returns(new List<RecursoOutput>
            {
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[0],
                    Id = TestUtils.ObjectMother.Guids[0],
                    LegacyId = TestUtils.ObjectMother.Ints[0]
                },
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[1],
                    Id = TestUtils.ObjectMother.Guids[1],
                    LegacyId = TestUtils.ObjectMother.Ints[1]
                },
                new RecursoOutput
                {
                    Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
                    Descricao = TestUtils.ObjectMother.Strings[2],
                    Id = TestUtils.ObjectMother.Guids[2],
                    LegacyId = TestUtils.ObjectMother.Ints[2]
                }
            });
        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            IdCategoria = TestUtils.ObjectMother.Guids[0],
            IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            Id = TestUtils.ObjectMother.Guids[0]
        }, true);

        await mocker.ClientesRepository.InsertAsync(new Cliente
        {
            Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            Id = TestUtils.ObjectMother.Guids[0],
            RazaoSocial = TestUtils.ObjectMother.Strings[0]
        }, true);

        var localDestino = new LocalOutput
        {
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Id = TestUtils.ObjectMother.Guids[0],
            IsBloquearMovimentacao = true
        };
        mocker.LocalProvider.GetById(TestUtils.ObjectMother.Guids[0]).Returns(localDestino);
        var ordemProducao = GetOrdemProducaoOutput(0);

        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(
            ordemProducao);
        
        var expectedMaquinas = new List<ExternalGerarOrdemRetrabalhoMaquinaInput>
        {
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "1",
                Sequencia = "1",
                DescricaoOperacao = input.MaquinasInput[0].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[0],
                QuantidadeHoras = 1.5m,
                ProdutividadeMaquina = 0,
                ApontarMaquina = true,
                Regula = 0,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Nivel = 0,
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "2",
                Sequencia = "1",
                DescricaoOperacao = input.MaquinasInput[1].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[1],
                QuantidadeHoras = 2,
                ProdutividadeMaquina = 0,
                ApontarMaquina = true,
                Regula = 0,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Nivel = 0,
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            },
            new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = "3",
                Sequencia = "1",
                DescricaoOperacao = input.MaquinasInput[2].Detalhamento,
                IdMaquina = TestUtils.ObjectMother.Ints[2],
                QuantidadeHoras = 12.5m,
                ProdutividadeMaquina = 0,
                ApontarMaquina = true,
                Regula = 0,
                Peca = TestUtils.ObjectMother.Ints[0].ToString(),
                PecaPai = "",
                Nivel = 0,
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            }
        };
        
        var expectedInput = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = TestUtils.ObjectMother.Ints[0],
            Quantidade = input.Quantidade,
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[0].ToString(),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, false),
            OdfOrigem = ordemProducao.NumeroOdf,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Maquinas = expectedMaquinas,
            Materias = new List<ExternalGerarOrdemRetrabalhoMaterialInput>()
        };
        
        //Act
        var result = await service.GetExternalGerarOrdemRetrabalhoInput(input);

        //Assert
        result.Should().BeEquivalentTo(expectedInput);
    }

    private GerarOrdemRetrabalhoInput GetGerarOrdemRetrabalhoInput(int index)
    {
        var gerarOrdemRetrabalhoInput = new GerarOrdemRetrabalhoInput
        {
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalDestino = TestUtils.ObjectMother.Guids[index],
            IdProduto = TestUtils.ObjectMother.Guids[index],
            NumeroLote = TestUtils.ObjectMother.Ints[index].ToString(),
            NumeroPedido = TestUtils.ObjectMother.Strings[index],
            IdPessoa = TestUtils.ObjectMother.Guids[index],
            NumeroOdfOrigem = TestUtils.ObjectMother.Ints[index],
            MateriaisInput = new List<GerarOrdemRetrabalhoMaterialInput>(),
            MaquinasInput = new List<GerarOrdemRetrabalhoMaquinaInput>()
        };
        return gerarOrdemRetrabalhoInput;
    }

    private OrdemProducaoOutput GetOrdemProducaoOutput(int index)
    {
        var ordemProducaoOutput = new OrdemProducaoOutput
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            Revisao = TestUtils.ObjectMother.Strings[index],
            IdProduto = TestUtils.ObjectMother.Guids[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            DataInicio = TestUtils.ObjectMother.Datas[index],
            DataEntrega = TestUtils.ObjectMother.Datas[index],
            NumeroPedido = TestUtils.ObjectMother.Strings[index],
            Observacao = TestUtils.ObjectMother.Strings[index],
            IsRetrabalho = false,
            NumeroOdfDestino = TestUtils.ObjectMother.Ints[index],
            OdfFinalizada = false
        };
        return ordemProducaoOutput;
    }
}