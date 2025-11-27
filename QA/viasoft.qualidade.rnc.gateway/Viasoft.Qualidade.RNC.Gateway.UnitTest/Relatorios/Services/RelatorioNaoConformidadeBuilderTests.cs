using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.Identity.Abstractions.Model;
using Viasoft.Core.MultiTenancy.Abstractions.Company.Model;
using Viasoft.Qualidade.RNC.Gateway.Host.Extensions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Dtos.DataSources;
using Viasoft.Qualidade.RNC.Gateway.Host.Relatorios.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Gateway.UnitTest.Relatorios.Services;

public class RelatorioNaoConformidadeBuilderTests
{
    [Fact(DisplayName = "Se relatorio construido com nao conformidade, deve montar NaoConformidadeDataSource")]
    public void WithNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.NaoConformidade = new NaoConformidadeDataSource
        {
            NaoConformidade = new List<RelatorioNaoConformidade>
            {
                GetRelatorioNaoConformidade(0)
            }
        };
        // Act
        var result = builder.WithNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com causas, deve montar CausaDataSource")]
    public void WithCausasNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();
        
        expectedResult.CausasNaoConformidade = new CausaDataSource()
        {
            CausasNaoConformidade = new List<CausaNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetCausaNaoConformidadeViewOutput(0)
            }
        };
        // Act
        var result = builder.WithCausasNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com defeitos, deve montar DefeitoDataSource")]
    public void WithDefeitosNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.DefeitosNaoConformidade = new DefeitoDataSource
        {
            DefeitosNaoConformidade = new List<DefeitoNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetDefeitoNaoConformidadeViewOutput(0)
            }
        };
        // Act
        var result = builder.WithDefeitosNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com solucoes, deve montar SolucaoDataSource")]
    public void WithSolucoesNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.SolucoesNaoConformidade = new SolucaoDataSource()
        {
            SolucoesNaoConformidade = new List<RelatorioSolucaoNaoConformidade>
            {
                new(TestUtils.ObjectMother.GetSolucaoNaoConformidadeViewOutput(0))
            }
        };
        // Act
        var result = builder.WithSolucoesNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com acoesPreventivas, deve montar AcaoPreventivaDataSource")]
    public void WithAcoesPreventivasNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.AcoesPreventivasNaoConformidade = new AcaoPreventivaDataSource
        {
            AcoesPreventivasNaoConformidade = new List<RelatorioAcaoPreventivaNaoConformidade>
            {
                new(TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidadeViewOutput(0))
            }
        };
        
        // Act
        var result = builder.WithAcoesPreventivasNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com implementacoes, deve montar ImplementacaoEvitarReincidenciaDataSource")]
    public void WithImplementacaoEvitarReincidenciaNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.ImplementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaDataSource()
        {
            ImplementacaoEvitarReincidenciaNaoConformidade = new List<RelatorioImplementacaoEvitarReincidenciaNaoConformidade>
            {
                new(TestUtils.ObjectMother.GetImplementacaoEvitarReincidenciaNaoConformidadeViewOutput(0))
            }
        };
        
        // Act
        var result = builder.WithImplementacaoEvitarReincidenciaNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com centro custo, deve montar CentroCustoCausaNaoConformidadeDataSource")]
    public void WithCentroCustoCausaNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.CentroCustoCausaNaoConformidade = new CentroCustoCausaNaoConformidadeDataSource()
        {
            CentroCustoCausaNaoConformidades = new List<CentroCustoCausaNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetCentroCustoCausaNaoConformidadeViewOutput(0)
            }
        };
        
        // Act
        var result = builder.WithCentroCustoCausaNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com ordem retrabalho, deve montar ordem retrabalho data source")]
    public void WithOrdemRetrabalhoNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.OrdemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidadeDataSource()
        {
            OrdemRetrabalhoNaoConformidade = new List<RelatorioOrdemRetrabalhoNaoConformidade>
            {
                GetRelatorioOrdemRetrabalhoNaoConformidade(0)
            }
        };
        
        // Act
        var result = builder.WithOrdemRetrabalhoNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se relatorio construido com operacao retrabalho, deve montar operacao retrabalho data source")]
    public void WithOperacaoRetrabalhoNaoConformidadeTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var builder = GetBuilder(dependencies);

        dependencies.DateTimeProvider.UtcNow().Returns(TestUtils.ObjectMother.Datas[0]);
        var expectedResult = new ExportarRelatorioNaoConformidadeInput();

        expectedResult.OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeDataSource
        {
            OperacaoRetrabalhoNaoConformidade = new List<OperacaoRetrabalhoNaoConformidade>
            {
                TestUtils.ObjectMother.GetOperacaoRetrabalhoNaoConformidade(0)
            },
        };

        expectedResult.OperacoesOperacaoRetrabalhoNaoConformidade =
            new OperacoesOperacaoRetrabalhoNaoConformidadeDataSource
            {
                OperacoesOperacaoRetrabalhoNaoConformidade =
                    new List<RelatorioOperacoesOperacaoRetrabalhoNaoConformidade>
                    {
                        GetRelatorioOperacoesOperacaoRetrabalhoNaoConformidade(0)
                    }
            };
        // Act
        var result = builder.WithOperacaoRetrabalhoNaoConformidade().Build();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    private RelatorioNaoConformidade GetRelatorioNaoConformidade(int index)
    {
        var relatorioNaoConformidade = new RelatorioNaoConformidade
        {
            CodigoRnc = TestUtils.ObjectMother.Ints[index].ToString(),
            Origem = OrigemNaoConformidade.Cliente.ToString(),
            Data = TestUtils.ObjectMother.Datas[index].ToLocal("America/Sao_Paulo").ToString(),
            NotaFiscal = TestUtils.ObjectMother.Ints[index].ToString(),
            Cliente = TestUtils.ObjectMother.Strings[index],
            Fornecedor = TestUtils.ObjectMother.Strings[index],
            Produto = TestUtils.ObjectMother.Strings[index],
            Lote = TestUtils.ObjectMother.Ints[index].ToString(),
            CodigoCliente = TestUtils.ObjectMother.Ints[index].ToString(),
            CodigoFornecedor = TestUtils.ObjectMother.Ints[index].ToString(),
            CodigoInterno = TestUtils.ObjectMother.Ints[index].ToString(),
            Revisao = TestUtils.ObjectMother.Ints[index].ToString(),
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhadoPeloCliente = false,
            RetrabalhadoNoCliente = false,
            TimeEnvolvido = TestUtils.ObjectMother.Strings[index],
            NaoConformidadePotencial = false,
            NaoConformidade = false,
            MelhoriaPotencial = false,
            DescricaoNaoConformidade = TestUtils.ObjectMother.Strings[index],
            ReclamacaoProcedente = TestUtils.ObjectMother.Ints[index].ToString(),
            ReclamacaoImprocedente = TestUtils.ObjectMother.Ints[index].ToString(),
            QuantidadeLote = TestUtils.ObjectMother.Ints[index].ToString(),
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[index].ToString(),
            AprovadoReclamacao = TestUtils.ObjectMother.Ints[index].ToString(),
            ConcessaoReclamacao = TestUtils.ObjectMother.Ints[index].ToString(),
            RejeitadoReclamacao = TestUtils.ObjectMother.Ints[index].ToString(),
            Retrabalho = TestUtils.ObjectMother.Ints[index].ToString(),
            RetrabalhoComOnus = false,
            RetrabalhoSemOnus = false,
            DevolucaoFornecedor = false,
            Recodificar = false,
            Sucata = false,
            Observacoes = TestUtils.ObjectMother.Strings[index],
            Reclamacoes = true,
            Conclusoes = true,
            Evidencia = TestUtils.ObjectMother.Strings[index],
            Eficaz = false,
            ClicloDeTempo = TestUtils.ObjectMother.Ints[index].ToString(),
            NomeUsuarioCriador = TestUtils.ObjectMother.Strings[index],
            SobrenomeUsuarioCriador = TestUtils.ObjectMother.Strings[index],
            DataCriacao = TestUtils.ObjectMother.Datas[index].ToString(),
            EmpresaId = TestUtils.ObjectMother.Guids[index].ToString(),
            EmpresaLegacyId = 1.ToString(),
            EmpresaCnpj = TestUtils.ObjectMother.Strings[index],
            EmpresaRazaoSocial = TestUtils.ObjectMother.Strings[index],
            EmpresaFantasia = TestUtils.ObjectMother.Strings[index],
        };
        return relatorioNaoConformidade;
    }

    private RelatorioOrdemRetrabalhoNaoConformidade GetRelatorioOrdemRetrabalhoNaoConformidade(int index)
    {
        var relatorioOrdemRetrabalhoNaoConformidade = new RelatorioOrdemRetrabalhoNaoConformidade
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[index],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[index],
            DescricaoLocalOrigem = TestUtils.ObjectMother.Strings[index],
            CodigoLocalOrigem = TestUtils.ObjectMother.Ints[index],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[index],
            IdLocalDestino = TestUtils.ObjectMother.Guids[index],
            DescricaoLocalDestino = TestUtils.ObjectMother.Strings[index],
            CodigoLocalDestino = TestUtils.ObjectMother.Ints[index],
            CodigoArmazem = TestUtils.ObjectMother.Strings[index],
            DataFabricacao = TestUtils.ObjectMother.Datas[index].ToString(),
            DataValidade = TestUtils.ObjectMother.Datas[index].ToString(),
            Status = StatusProducaoRetrabalho.Aberta.ToString()
        };
        return relatorioOrdemRetrabalhoNaoConformidade;
    }

    private RelatorioOperacoesOperacaoRetrabalhoNaoConformidade
        GetRelatorioOperacoesOperacaoRetrabalhoNaoConformidade(int index)
    {
        var relatorioOperacoesOperacaoRetrabalhoNaoConformidade =
            new RelatorioOperacoesOperacaoRetrabalhoNaoConformidade
            {
                Id = TestUtils.ObjectMother.Guids[index],
                NumeroOperacao = TestUtils.ObjectMother.Ints[index].ToString(),
                IdRecurso = TestUtils.ObjectMother.Guids[index],
                DescricaoRecurso = TestUtils.ObjectMother.Strings[index],
                CodigoRecurso = TestUtils.ObjectMother.Ints[index].ToString(),
                IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[index],
                Status = StatusProducaoRetrabalho.Aberta.ToString()
            };
        return relatorioOperacoesOperacaoRetrabalhoNaoConformidade;
    }

    private OrdemRetrabalhoNaoConformidadeViewOutput GetOrdemRetrabalhoNaoConformidadeViewOutput(int index)
    {
        var ordemRetrabalhoNaoConformidadeViewOutput = new OrdemRetrabalhoNaoConformidadeViewOutput
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[index],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[index],
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[index],
            DescricaoLocalOrigem = TestUtils.ObjectMother.Strings[index],
            CodigoLocalOrigem = TestUtils.ObjectMother.Ints[index],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[index],
            IdLocalDestino = TestUtils.ObjectMother.Guids[index],
            DescricaoLocalDestino = TestUtils.ObjectMother.Strings[index],
            CodigoLocalDestino = TestUtils.ObjectMother.Ints[index],
            CodigoArmazem = TestUtils.ObjectMother.Strings[index],
            DataFabricacao = TestUtils.ObjectMother.Datas[index],
            DataValidade = TestUtils.ObjectMother.Datas[index],
            Status = StatusProducaoRetrabalho.Aberta
        };
        return ordemRetrabalhoNaoConformidadeViewOutput;
    }

    private GroupedReadModels GetGroupedReadModels(int index)
    {
        var groupedReadModels = new GroupedReadModels
        {
            NaoConformidadeViewOutput = TestUtils.ObjectMother.GetNaoConformidadeViewOutput(index),
            CausaNaoConformidadeViewOutput = new List<CausaNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetCausaNaoConformidadeViewOutput(index)
            },
            SolucaoNaoConformidadeViewOutput = new List<SolucaoNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetSolucaoNaoConformidadeViewOutput(index)
            },
            DefeitoNaoConformidadeViewOutput = new List<DefeitoNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetDefeitoNaoConformidadeViewOutput(index)
            },
            AcaoPreventivaNaoConformidadeViewOutput = new List<AcaoPreventivaNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidadeViewOutput(index)
            },
            ImplementacaoEvitarReincidenciaNaoConformidadeViewOutputs =
                new List<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>
                {
                    TestUtils.ObjectMother.GetImplementacaoEvitarReincidenciaNaoConformidadeViewOutput(index)
                },
            CentroCustoCausaNaoConformidadeViewOutputs = new List<CentroCustoCausaNaoConformidadeViewOutput>
            {
                TestUtils.ObjectMother.GetCentroCustoCausaNaoConformidadeViewOutput(index)
            },
            OrdemRetrabalhoNaoConformidadeViewOutput = GetOrdemRetrabalhoNaoConformidadeViewOutput(0),
            OperacaoRetrabalhoNaoConformidade = TestUtils.ObjectMother.GetOperacaoRetrabalhoNaoConformidade(0),
            OperacoesOperacaoRetrabalhoNaoConformidade = new List<OperacaoViewOutput>
            {
                TestUtils.ObjectMother.GetOperacaoViewOutput(0)
            }
        };
        return groupedReadModels;
    }

    private class Dependencies
    {
        public IDateTimeProvider DateTimeProvider { get; set; }
        public AgregacaoNaoConformidadeOutput AgregacaoNaoConformidadeOutput { get; set; }
        public UserPreferences UserPreferences { get; set; }
        public GroupedReadModels GroupedReadModels { get; set; }
        public CompanyDetails Empresa { get; set; }
    }

    private Dependencies GetDependencies()
    {
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        var dependecies = new Dependencies
        {
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            AgregacaoNaoConformidadeOutput = agregacao,
            UserPreferences = TestUtils.ObjectMother.GetUserPreferences(0),
            GroupedReadModels = GetGroupedReadModels(0),
            Empresa = TestUtils.ObjectMother.GetEmpresa(0),
        };
        return dependecies;
    }

    private RelatorioNaoConformidadeBuilder GetBuilder(Dependencies dependencies)
    {
        var relatorioNaoConformidadeBuilder = new RelatorioNaoConformidadeBuilder(dependencies.DateTimeProvider,
            dependencies.AgregacaoNaoConformidadeOutput, dependencies.UserPreferences, dependencies.GroupedReadModels,
            dependencies.Empresa);
        return relatorioNaoConformidadeBuilder;
    }
}
