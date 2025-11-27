using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.OperacoesRetrabalhos.Services;

public class OperacaoRetrabalhoAclServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se método chamado, deve converter o input para GerarOperacaoRetrabalhoExternalInput")]
    public async Task GetGerarOperacaoRetrabalhoExternalInputTest()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var gerarOperacaoRetrabalhoAclInput = new GerarOperacaoRetrabalhoAclInput
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeModel
            {
                NumeroOperacaoARetrabalhar = "010",
                Quantidade = 100,
                Id = TestUtils.ObjectMother.Guids[0]
            }
        };

        var operacaoAclInput = new List<MaquinaInput>
        {
            new MaquinaInput
            {
                Horas = 12,
                Minutos = 0,
                Detalhamento = TestUtils.ObjectMother.Strings[0],
                IdRecurso = TestUtils.ObjectMother.Guids[0],
                Materiais = new List<MaterialInput>()
            },
            new MaquinaInput
            {
                Horas = 10,
                Minutos = 12,
                Detalhamento = TestUtils.ObjectMother.Strings[1],
                IdRecurso = TestUtils.ObjectMother.Guids[1],
                Materiais = new List<MaterialInput>()
            },
        };

        await mocker.Recursos.InsertRangeAsync(new List<Recurso>
        {
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            },
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
            }
        });
        await UnitOfWork.SaveChangesAsync();
        
        var expectedResult = new GerarOperacaoRetrabalhoExternalInput
        {
            IdOrigem = TestUtils.ObjectMother.Guids[0],
            Odf = TestUtils.ObjectMother.Ints[0],
            Operacao = "010",
            SaldoRetrabalhar = 100,
            Operacoes = new List<OperacaoRetrabalhoExternalInput>
            {
                new OperacaoRetrabalhoExternalInput
                {
                    Hora = 12,
                    Minuto = 0,
                    DescricaoOperacao = TestUtils.ObjectMother.Strings[0],
                    Maquina = TestUtils.ObjectMother.Ints[0].ToString(),
                    Segundo = 0,
                    Materiais = new List<MaterialRetrabalhoExternalInput>()
                },
                new OperacaoRetrabalhoExternalInput
                {
                    Hora = 10,
                    Minuto = 12,
                    DescricaoOperacao = TestUtils.ObjectMother.Strings[1],
                    Maquina = TestUtils.ObjectMother.Ints[1].ToString(),
                    Segundo = 0,
                    Materiais = new List<MaterialRetrabalhoExternalInput>()
                }
            }
        };
       
        // Act
        var output = await service.GetGerarOperacaoRetrabalhoExternalInput(gerarOperacaoRetrabalhoAclInput, operacaoAclInput);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    } 
    
    [Fact(DisplayName = "Se houver materiais, deve converte-los e devolver GerarOperacaoRetrabalhoExternalInput")]
    public async Task GetGerarOperacaoRetrabalhoExternalInputTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var gerarOperacaoRetrabalhoAclInput = new GerarOperacaoRetrabalhoAclInput
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeModel
            {
                NumeroOperacaoARetrabalhar = "010",
                Quantidade = 100,
                Id = TestUtils.ObjectMother.Guids[0]
            }
        };

        var operacaoAclInput = new List<MaquinaInput>
        {
            new MaquinaInput
            {
                Horas = 12,
                Minutos = 0,
                Detalhamento = TestUtils.ObjectMother.Strings[0],
                IdRecurso = TestUtils.ObjectMother.Guids[0],
                Materiais = new List<MaterialInput>
                {
                    new MaterialInput
                    {
                        Id = TestUtils.ObjectMother.Guids[0],
                        Quantidade = TestUtils.ObjectMother.Ints[0],
                        IdProduto = TestUtils.ObjectMother.Guids[0],
                        Detalhamento = TestUtils.ObjectMother.Strings[0],
                    },
                    new MaterialInput
                    {
                        Id = TestUtils.ObjectMother.Guids[1],
                        Quantidade = TestUtils.ObjectMother.Ints[1],
                        IdProduto = TestUtils.ObjectMother.Guids[1],
                        Detalhamento = TestUtils.ObjectMother.Strings[1],
                    },
                }
            },
            new MaquinaInput
            {
                Horas = 10,
                Minutos = 12,
                Detalhamento = TestUtils.ObjectMother.Strings[1],
                IdRecurso = TestUtils.ObjectMother.Guids[1],
                Materiais = new List<MaterialInput>
                {
                    new MaterialInput
                    {
                        Id = TestUtils.ObjectMother.Guids[0],
                        Quantidade = TestUtils.ObjectMother.Ints[0],
                        IdProduto = TestUtils.ObjectMother.Guids[0],
                        Detalhamento = TestUtils.ObjectMother.Strings[0],
                    },
                    new MaterialInput
                    {
                        Id = TestUtils.ObjectMother.Guids[1],
                        Quantidade = TestUtils.ObjectMother.Ints[1],
                        IdProduto = TestUtils.ObjectMother.Guids[1],
                        Detalhamento = TestUtils.ObjectMother.Strings[1],
                    },
                }
            },
        };

        await mocker.Recursos.InsertRangeAsync(new List<Recurso>
        {
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            },
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
            }
        });
        await mocker.Produtos.InsertRangeAsync(new List<Produto>
        {
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[0],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[0],
            },
            new Produto
            {
                Id = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                IdCategoria = TestUtils.ObjectMother.Guids[1],
                IdUnidadeMedida = TestUtils.ObjectMother.Guids[1],
            }
        });
        await UnitOfWork.SaveChangesAsync();
        
        var expectedResult = new GerarOperacaoRetrabalhoExternalInput
        {
            IdOrigem = TestUtils.ObjectMother.Guids[0],
            Odf = TestUtils.ObjectMother.Ints[0],
            Operacao = "010",
            SaldoRetrabalhar = 100,
            Operacoes = new List<OperacaoRetrabalhoExternalInput>
            {
                new OperacaoRetrabalhoExternalInput
                {
                    Hora = 12,
                    Minuto = 0,
                    DescricaoOperacao = TestUtils.ObjectMother.Strings[0],
                    Maquina = TestUtils.ObjectMother.Ints[0].ToString(),
                    Segundo = 0,
                    Materiais = new List<MaterialRetrabalhoExternalInput>
                    {
                        new MaterialRetrabalhoExternalInput
                        {
                            Quantidade = TestUtils.ObjectMother.Ints[0],
                            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString()
                        },
                        new MaterialRetrabalhoExternalInput
                        {
                            Quantidade = TestUtils.ObjectMother.Ints[1],
                            CodigoProduto = TestUtils.ObjectMother.Ints[1].ToString()
                        }
                    }
                },
                new OperacaoRetrabalhoExternalInput
                {
                    Hora = 10,
                    Minuto = 12,
                    DescricaoOperacao = TestUtils.ObjectMother.Strings[1],
                    Maquina = TestUtils.ObjectMother.Ints[1].ToString(),
                    Segundo = 0,
                    Materiais = new List<MaterialRetrabalhoExternalInput>
                    {
                        new MaterialRetrabalhoExternalInput
                        {
                            Quantidade = TestUtils.ObjectMother.Ints[0],
                            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString()
                        },
                        new MaterialRetrabalhoExternalInput
                        {
                            Quantidade = TestUtils.ObjectMother.Ints[1],
                            CodigoProduto = TestUtils.ObjectMother.Ints[1].ToString()
                        }
                    }
                }
            }
        };
        
        // Act
        var output = await service.GetGerarOperacaoRetrabalhoExternalInput(gerarOperacaoRetrabalhoAclInput, operacaoAclInput);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);

    }
    
    [Fact(DisplayName = "Se sucesso ao criar operação de retrabalho, deve converter as operações adicionadas")]
    public async Task GetGerarOperacaoRetrabalhoAclOutputTest()
          {
              // Arrange
              var mocker = GetMocker();
              var service = GetService(mocker);
              
              await mocker.Recursos.InsertRangeAsync(new List<Recurso>
              {
                  new Recurso
                  {
                      Id = TestUtils.ObjectMother.Guids[0],
                      Descricao = TestUtils.ObjectMother.Strings[0],
                      Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                  },
                  new Recurso
                  {
                      Id = TestUtils.ObjectMother.Guids[1],
                      Descricao = TestUtils.ObjectMother.Strings[1],
                      Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                  }
              });
              await UnitOfWork.SaveChangesAsync();
              
              var input = new GerarOperacaoRetrabalhoExternalOutput
              {
                OperacoesAdicionadas = new List<OperacaoRetrabalhoExternalOutput>
                {
                    new OperacaoRetrabalhoExternalOutput
                    {
                        Maquina = TestUtils.ObjectMother.Ints[0].ToString(),
                        Operacao = "011"
                    },
                    new OperacaoRetrabalhoExternalOutput
                    {
                        Maquina = TestUtils.ObjectMother.Ints[1].ToString(),
                        Operacao = "012"
                    },
                }
              };
              var expectedResult = new GerarOperacaoRetrabalhoAclOutput
              {
                  Success = true,
                  OperacoesAdicionadas = new List<OperacaoAclOutput>
                  {
                      new OperacaoAclOutput
                      {
                          NumeroOperacao = "011",
                          IdMaquina = TestUtils.ObjectMother.Guids[0]
                      },
                      new OperacaoAclOutput
                      {
                          NumeroOperacao = "012",
                          IdMaquina = TestUtils.ObjectMother.Guids[1]
                      }
                  },
                  ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok
              };
              // Act
              var output = await service.GetGerarOperacaoRetrabalhoAclOutput(input);
              
              //Assert
              output.Should().BeEquivalentTo(expectedResult);
          } 
    
    [Fact(DisplayName = "Se falha ao criar operação de retrabalho, deve converter code em validation result")]
    public async Task GetGerarOperacaoRetrabalhoAclOutputTest2()
          {
              // Arrange
              var mocker = GetMocker();
              var service = GetService(mocker);
              
              await mocker.Recursos.InsertRangeAsync(new List<Recurso>
              {
                  new Recurso
                  {
                      Id = TestUtils.ObjectMother.Guids[0],
                      Descricao = TestUtils.ObjectMother.Strings[0],
                      Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
                  },
                  new Recurso
                  {
                      Id = TestUtils.ObjectMother.Guids[1],
                      Descricao = TestUtils.ObjectMother.Strings[1],
                      Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
                  }
              });
              await UnitOfWork.SaveChangesAsync();
              
              var input = new GerarOperacaoRetrabalhoExternalOutput
              {
                  Message = "Atenção! Odf informado possui saldo zerado.",
                  Code = 1051
              };
              var expectedResult = new GerarOperacaoRetrabalhoAclOutput
              {
                  Message = "Atenção! Odf informado possui saldo zerado.",
                  Success = false,
                  OperacoesAdicionadas = new List<OperacaoAclOutput>(),
                  ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.GerarRetrabalhoOpSecundariaSaldoOdfZerado
              };
              // Act
              var output = await service.GetGerarOperacaoRetrabalhoAclOutput(input);
              
              //Assert
              output.Should().BeEquivalentTo(expectedResult);
          } 
    
    private class Mocker
    {
        public IRepository<Produto> Produtos { get; set; }
        public IRepository<Recurso> Recursos { get; set; }
    }

    private Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            Produtos = ServiceProvider.GetService<IRepository<Produto>>(),
            Recursos = ServiceProvider.GetService<IRepository<Recurso>>()
        };

        return mocker;
    }

    private OperacaoRetrabalhoAclService GetService(Mocker mocker)
    {
        var service = new OperacaoRetrabalhoAclService(mocker.Produtos, mocker.Recursos);

        return service;
    }
}