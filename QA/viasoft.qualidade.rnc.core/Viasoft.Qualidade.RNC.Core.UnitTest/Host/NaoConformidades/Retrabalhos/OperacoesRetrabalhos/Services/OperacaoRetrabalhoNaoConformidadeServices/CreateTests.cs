using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeServices;

public class CreateTests : OperacaoRetrabalhoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Se operação de retrabalho já criada, deve retornar mensagem com o  erro")]
    public async Task CreateTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100
        };

        mocker.OperacaoRetrabalhoNaoConformidadeValidatorService
            .ValidateOperacaoRetrabalhoJaExistente(agregacaoNaoConformidade)
            .Returns(OperacaoRetrabalhoNaoConformidadeValidationResult.OperacaoRetrabalhoJaExiste);

        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);

        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "Operação de retrabalho já criada",
            ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.OperacaoRetrabalhoJaExiste
        };
        // Act
        var output = await service.Create(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se serviços não estiverem validos, deve retornar mensagem com o erro")]
    public async Task CreateTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100
        };

        mocker.OperacaoRetrabalhoNaoConformidadeValidatorService
            .ValidateMaquina(input)
            .Returns(OperacaoRetrabalhoNaoConformidadeValidationResult.NenhumMaquinaCadastrada);

        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);

        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "É necessário ao menos uma máquina para gerar operação de retrabalho",
            ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.NenhumMaquinaCadastrada
        };
        // Act
        var output = await service.Create(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se odf não apontada, deve retornar mensagem com o erro")]
    public async Task CreateTest3()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100
        };

        mocker.OperacaoRetrabalhoNaoConformidadeValidatorService
            .ValidateOdfApontada(agregacaoNaoConformidade)
            .Returns(OperacaoRetrabalhoNaoConformidadeValidationResult.OdfNaoApontada);

        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);
        
        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Success = false,
            Message = "A Odf informada, ainda não foi apontada",
            ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.OdfNaoApontada
        };
        // Act
        var output = await service.Create(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se método chamado, deve buscar o input e chamar criação de operação de retrabalho")]
    public async Task CreateTest4()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100,
            Maquinas = new List<MaquinaInput>
            {
                new MaquinaInput
                {
                    Horas = TestUtils.ObjectMother.Ints[0],
                    Minutos = TestUtils.ObjectMother.Ints[0],
                    IdRecurso = TestUtils.ObjectMother.Guids[0],
                    Detalhamento = TestUtils.ObjectMother.Strings[0],
                    Materiais = new List<MaterialInput>
                    {
                        new MaterialInput
                        {
                            Id = TestUtils.ObjectMother.Guids[0],
                            IdProduto = TestUtils.ObjectMother.Guids[0],
                            Quantidade = TestUtils.ObjectMother.Ints[0],
                            Detalhamento = TestUtils.ObjectMother.Strings[0],
                        }
                    }
                }
            }
        };

        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);

        var expectedGerarOperacaoRetrabalhoAclInput = new GerarOperacaoRetrabalhoAclInput
        {
            NumeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf,
            OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeModel
            {
                NumeroOperacaoARetrabalhar = "010",
                Quantidade = 100
            }
        };
        var expectedOperacoes = new List<MaquinaInput>
        {
           new MaquinaInput
           {
               Horas = TestUtils.ObjectMother.Ints[0],
               Minutos = TestUtils.ObjectMother.Ints[0],
               IdRecurso = TestUtils.ObjectMother.Guids[0],
               Detalhamento = TestUtils.ObjectMother.Strings[0],
               Materiais = new List<MaterialInput>
               {
                   new MaterialInput
                   {
                       Id = TestUtils.ObjectMother.Guids[0],
                       IdProduto = TestUtils.ObjectMother.Guids[0],
                       Quantidade = TestUtils.ObjectMother.Ints[0],
                       Detalhamento = TestUtils.ObjectMother.Strings[0],
                   }
               }
           }
        };
        var gerarOperacaoRetrabalhoExternalInput = new GerarOperacaoRetrabalhoExternalInput()
        {
            IdOrigem = TestUtils.ObjectMother.Guids[0],
            Operacoes = new List<OperacaoRetrabalhoExternalInput>
            {
                
            },
            SaldoRetrabalhar = 10,
            Operacao = "010",
            Odf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value
        };
        mocker.OperacaoRetrabalhoAclService.GetGerarOperacaoRetrabalhoExternalInput(
            Arg.Is<GerarOperacaoRetrabalhoAclInput>(e => e.IsEquivalentTo(expectedGerarOperacaoRetrabalhoAclInput, 
                new List<string>
            {
                nameof(GerarOperacaoRetrabalhoAclInput.OperacaoRetrabalhoNaoConformidade.Id)
            })),
            Arg.Is<List<MaquinaInput>>(e => e.IsEquivalentTo(expectedOperacoes)))
            .Returns(gerarOperacaoRetrabalhoExternalInput);
        
        var gerarOperacaoRetrabalhoExternalOutput = new GerarOperacaoRetrabalhoExternalOutput
        {
            Message = "Deu erro",
            OperacoesAdicionadas = new List<OperacaoRetrabalhoExternalOutput>()
        };
        mocker.OperacaoRetrabalhoProxyService
            .Create(Arg.Is<GerarOperacaoRetrabalhoExternalInput>(e => 
                e.IsEquivalentTo(gerarOperacaoRetrabalhoExternalInput))).Returns(gerarOperacaoRetrabalhoExternalOutput);
        
        mocker.OperacaoRetrabalhoAclService.GetGerarOperacaoRetrabalhoAclOutput(gerarOperacaoRetrabalhoExternalOutput).Returns(
            new GerarOperacaoRetrabalhoAclOutput
            {
                Message = "Deu erro"
            });
        // Act
        var output = await service.Create(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        await mocker.OperacaoRetrabalhoProxyService
            .Received(1)
            .Create(Arg.Is<GerarOperacaoRetrabalhoExternalInput>(e =>
                e.IsEquivalentTo(gerarOperacaoRetrabalhoExternalInput)));
    }
    
    [Fact(DisplayName = "Se sucesso na geração da operação, deve salvar a operacaoRetrabalhoNaoConformidade")]
    public async Task CreateTest5()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100,
            Maquinas = new List<MaquinaInput>
            {
                new MaquinaInput()
                {
                    Horas = TestUtils.ObjectMother.Ints[0],
                    Minutos = TestUtils.ObjectMother.Ints[0],
                    IdRecurso = TestUtils.ObjectMother.Guids[0],
                    Detalhamento = TestUtils.ObjectMother.Strings[0],
                    Materiais = new List<MaterialInput>
                    {
                        new MaterialInput
                        {
                            Id = TestUtils.ObjectMother.Guids[0],
                            IdProduto = TestUtils.ObjectMother.Guids[0],
                            Quantidade = TestUtils.ObjectMother.Ints[0],
                            Detalhamento = TestUtils.ObjectMother.Strings[0],
                        }
                    }
                }
            }
        };

        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);
        
        var naoConformidadesRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>();

        var naoConformidade = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        };
        await naoConformidadesRepository.InsertAsync(naoConformidade);
        
        await UnitOfWork.SaveChangesAsync();

        var expectedGerarOperacaoRetrabalhoAclInput = new GerarOperacaoRetrabalhoAclInput
        {
            NumeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf,
            OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeModel
            {
                NumeroOperacaoARetrabalhar = "010",
                Quantidade = 100
            }
        };
        var expectedOperacoesInput = new List<MaquinaInput>
        {
           new MaquinaInput()
           {
               Horas = TestUtils.ObjectMother.Ints[0],
               Minutos = TestUtils.ObjectMother.Ints[0],
               IdRecurso = TestUtils.ObjectMother.Guids[0],
               Detalhamento = TestUtils.ObjectMother.Strings[0],
               Materiais = new List<MaterialInput>
               {
                   new MaterialInput
                   {
                       Id = TestUtils.ObjectMother.Guids[0],
                       IdProduto = TestUtils.ObjectMother.Guids[0],
                       Quantidade = TestUtils.ObjectMother.Ints[0],
                       Detalhamento = TestUtils.ObjectMother.Strings[0],
                   }
               }
           }
        };
        var gerarOperacaoRetrabalhoExternalInput = new GerarOperacaoRetrabalhoExternalInput()
        {
            IdOrigem = TestUtils.ObjectMother.Guids[0],
            Operacoes = new List<OperacaoRetrabalhoExternalInput>
            {
                
            },
            SaldoRetrabalhar = 10,
            Operacao = "010",
            Odf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value
        };
        mocker.OperacaoRetrabalhoAclService.GetGerarOperacaoRetrabalhoExternalInput(
                Arg.Is<GerarOperacaoRetrabalhoAclInput>(e => e.IsEquivalentTo(expectedGerarOperacaoRetrabalhoAclInput, 
                    new List<string>
                    {
                        nameof(GerarOperacaoRetrabalhoAclInput.OperacaoRetrabalhoNaoConformidade.Id)
                    })),
                Arg.Is<List<MaquinaInput>>(e => e.IsEquivalentTo(expectedOperacoesInput)))
            .Returns(gerarOperacaoRetrabalhoExternalInput);

        var operacoesAdicionadas = new List<OperacaoRetrabalhoExternalOutput>
        {
            new OperacaoRetrabalhoExternalOutput
            {
                Operacao = "011",
                Maquina = TestUtils.ObjectMother.Strings[0]
            }
        };
        mocker.OperacaoRetrabalhoProxyService
            .Create(Arg.Is<GerarOperacaoRetrabalhoExternalInput>(e =>
                e.IsEquivalentTo(gerarOperacaoRetrabalhoExternalInput)))
            .Returns(new GerarOperacaoRetrabalhoExternalOutput
            {
                OperacoesAdicionadas = operacoesAdicionadas
            });
        var expectedGerarOperacaoRetrabalhoExternalOutput = new GerarOperacaoRetrabalhoExternalOutput
        {
            OperacoesAdicionadas = operacoesAdicionadas,
        };
        mocker.OperacaoRetrabalhoAclService
            .GetGerarOperacaoRetrabalhoAclOutput(
                Arg.Is<GerarOperacaoRetrabalhoExternalOutput>(e => e.IsEquivalentTo(expectedGerarOperacaoRetrabalhoExternalOutput)))
            .Returns(new GerarOperacaoRetrabalhoAclOutput
            {
                OperacoesAdicionadas = new List<OperacaoAclOutput>
                {
                    new OperacaoAclOutput
                    {
                        IdMaquina = TestUtils.ObjectMother.Guids[0],
                        NumeroOperacao = "011",
                    }
                },
                Success = true
            });
        var expectedResult = new OperacaoRetrabalhoNaoConformidade
        {
            Quantidade = 100,
            NumeroOperacaoARetrabalhar = "010",
            IdNaoConformidade = agregacaoNaoConformidade.NaoConformidade.Id,
            NaoConformidade = naoConformidade,
            Operacoes = new List<Operacao>
            {
                new Operacao
                {
                    IdRecurso = TestUtils.ObjectMother.Guids[0],
                    NumeroOperacao = "011",
                    Status = StatusProducaoRetrabalho.Aberta
                }
            }
        };
        
        // Act
        var output = await service.Create(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        var operacaoRetrabalhoResult = await mocker.OperacaoRetrabalhoNaoConformidades.FirstAsync(e =>
            e.IdNaoConformidade == agregacaoNaoConformidade.NaoConformidade.Id);
        
        foreach (var operacao in expectedResult.Operacoes)
        {
            operacao.IdOperacaoRetrabalhoNaoConformdiade = operacaoRetrabalhoResult.Id;
            operacao.OperacaoRetrabalhoNaoConformidade = operacaoRetrabalhoResult;
        }
        operacaoRetrabalhoResult.Should().BeEquivalentTo(expectedResult, options => TestUtils
            .ExcludeAuditoria(options)
            .Excluding(e => e.Operacoes[0].Id)
            .Excluding(e => e.Operacoes[0].CreationTime)
            .Excluding(e => e.Operacoes[0].CreatorId)
            .Excluding(e => e.Id));
    }
    
    [Fact(DisplayName = "Se falhar ao gerar operação, deve retornar mensagem com o erro")]
    public async Task CreateTest6()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var input = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = "010",
            Quantidade = 100,
            Maquinas = new List<MaquinaInput>
            {
                new MaquinaInput
                {
                    Horas = TestUtils.ObjectMother.Ints[0],
                    Minutos = TestUtils.ObjectMother.Ints[0],
                    IdRecurso = TestUtils.ObjectMother.Guids[0],
                    Detalhamento = TestUtils.ObjectMother.Strings[0],
                    Materiais = new List<MaterialInput>
                    {
                        new MaterialInput()
                        {
                            Id = TestUtils.ObjectMother.Guids[0],
                            IdProduto = TestUtils.ObjectMother.Guids[0],
                            Quantidade = TestUtils.ObjectMother.Ints[0],
                            Detalhamento = TestUtils.ObjectMother.Strings[0],
                        }
                    }
                }
            }
        };

        mocker.NaoConformidadeRepository
            .Operacoes()
            .Get(agregacaoNaoConformidade.NaoConformidade.Id)
            .Returns(agregacaoNaoConformidade);
        
        var naoConformidadesRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>();

        var naoConformidade = new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        };
        await naoConformidadesRepository.InsertAsync(naoConformidade);
        
        await UnitOfWork.SaveChangesAsync();

        var expectedGerarOperacaoRetrabalhoAclInput = new GerarOperacaoRetrabalhoAclInput
        {
            NumeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf,
            OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeModel
            {
                Id = TestUtils.ObjectMother.Guids[0],
                NumeroOperacaoARetrabalhar = "010",
                Quantidade = 100
            }
        };
        var expectedOperacoesInput = new List<MaquinaInput>
        {
           new MaquinaInput
           {
               Horas = TestUtils.ObjectMother.Ints[0],
               Minutos = TestUtils.ObjectMother.Ints[0],
               IdRecurso = TestUtils.ObjectMother.Guids[0],
               Detalhamento = TestUtils.ObjectMother.Strings[0],
               Materiais = new List<MaterialInput>
               {
                   new MaterialInput()
                   {
                       Id = TestUtils.ObjectMother.Guids[0],
                       IdProduto = TestUtils.ObjectMother.Guids[0],
                       Quantidade = TestUtils.ObjectMother.Ints[0],
                       Detalhamento = TestUtils.ObjectMother.Strings[0],
                   }
               }
           }
        };
       
        var gerarOperacaoRetrabalhoExternalInput = new GerarOperacaoRetrabalhoExternalInput()
        {
            IdOrigem = TestUtils.ObjectMother.Guids[0],
            Operacoes = new List<OperacaoRetrabalhoExternalInput>
            {
                
            },
            SaldoRetrabalhar = 10,
            Operacao = "010",
            Odf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value
        };
        mocker.OperacaoRetrabalhoAclService
            .GetGerarOperacaoRetrabalhoExternalInput(Arg.Is<GerarOperacaoRetrabalhoAclInput>(e => 
                e.IsEquivalentTo(expectedGerarOperacaoRetrabalhoAclInput, new List<string>
                {
                    nameof(OperacaoRetrabalhoNaoConformidadeModel.Id)
                })), 
                Arg.Is<List<MaquinaInput>>(e => e.IsEquivalentTo(expectedOperacoesInput)))
            .Returns(gerarOperacaoRetrabalhoExternalInput);
        
        var expectedGerarOperacaoRetrabalhoExternalOutput = new GerarOperacaoRetrabalhoExternalOutput
        {
            Message = "Erro ao gerar operação de retrabalho",
            OperacoesAdicionadas = new List<OperacaoRetrabalhoExternalOutput>()
        };
        
        mocker.OperacaoRetrabalhoProxyService
            .Create(Arg.Is<GerarOperacaoRetrabalhoExternalInput>(e =>
                e.IsEquivalentTo(gerarOperacaoRetrabalhoExternalInput)))
            .Returns(expectedGerarOperacaoRetrabalhoExternalOutput);

        mocker.OperacaoRetrabalhoAclService.GetGerarOperacaoRetrabalhoAclOutput(Arg.Is<GerarOperacaoRetrabalhoExternalOutput>(
            e => e.IsEquivalentTo(expectedGerarOperacaoRetrabalhoExternalOutput))).Returns(
            new GerarOperacaoRetrabalhoAclOutput
            {
                Success = false,
                Message = "Erro ao gerar operação de retrabalho"
            });
        var expectedResult = new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Message = "Erro ao gerar operação de retrabalho",
            Success = false,
        };       
        
        // Act
        var output = await service.Create(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
}