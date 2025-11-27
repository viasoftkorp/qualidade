using System;
using System.Collections.Generic;
using NSubstitute;
using Viasoft.Core.Identity.Abstractions.Model;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Company.Model;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.UnitTest;

public class TestUtils
{
    public class ObjectMother
    {
        public class AgregacaoNaoConformidadeMock
        {
            public AgregacaoNaoConformidadeOutput AgregacaoFromThis()
            {
                return new AgregacaoNaoConformidadeOutput(NaoConformidade, AcaoPreventivaNaoConformidades,
                    CausaNaoConformidades, DefeitoNaoConformidades, SolucaoNaoConformidades,
                    new List<ProdutoNaoConformidadeOutput>(), new List<ServicoNaoConformidadeOutput>(), CentroCustoCausaNaoConformidades,
                    ConclusaoNaoConformidade, ReclamacaoNaoConformidade, ImplementacoesEvitarReincidenciaNaoConformidades);
            }

            public NaoConformidadeOutput NaoConformidade { get; set; }
            public ConclusaoNaoConformidadeOutput ConclusaoNaoConformidade { get; set; }
            public ReclamacaoNaoConformidadeOutput ReclamacaoNaoConformidade { get; set; }
            public List<AcaoPreventivaNaoConformidadeOutput> AcaoPreventivaNaoConformidades { get; set; }
            public List<CausaNaoConformidadeOutput> CausaNaoConformidades { get; set; }
            public List<DefeitoNaoConformidadeOutput> DefeitoNaoConformidades { get; set; }
            public List<SolucaoNaoConformidadeOutput> SolucaoNaoConformidades { get; set; }
            public List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput> ImplementacoesEvitarReincidenciaNaoConformidades { get; set; }
            public List<CentroCustoCausaNaoConformidadeOutput> CentroCustoCausaNaoConformidades { get; set; }

            public AgregacaoNaoConformidadeMock()
            {
                AcaoPreventivaNaoConformidades = new List<AcaoPreventivaNaoConformidadeOutput>();
                CausaNaoConformidades = new List<CausaNaoConformidadeOutput>();
                DefeitoNaoConformidades = new List<DefeitoNaoConformidadeOutput>();
                SolucaoNaoConformidades = new List<SolucaoNaoConformidadeOutput>();
                ImplementacoesEvitarReincidenciaNaoConformidades =
                    new List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput>();
            }
        }

        public static AgregacaoNaoConformidadeMock GetAgregacaoNaoConformidadeMock(int index)
        {
            var acoesPreventivas = new List<AcaoPreventivaNaoConformidadeOutput>
            {
                GetAcaoPreventivaNaoConformidadeOutput(index)
            };
            var causas = new List<CausaNaoConformidadeOutput>
            {
                GetCausaNaoConformidadeOutput(index)
            };
            var defeitosNaoConformidadeOutput = new List<DefeitoNaoConformidadeOutput>
            {
                GetDefeitoNaoConformidadeOutput(index)
            };
            var solucoesNaoConformidadeOutput = new List<SolucaoNaoConformidadeOutput>
            {
                GetSolucaoNaoConformidadeOutput(index)
            };

            var centrosCustoNaoConformidadeOutputs = new List<CentroCustoCausaNaoConformidadeOutput>
            {
                GetCentroCustoCausaNaoConformidadeDto(index)
            };
            var implementacoes = new List<ImplementacaoEvitarReincidenciaNaoConformidadeOutput>
            {
                GetImplementacaoEvitarReincidenciaNaoConformidadeDto(0)
            };
            var agregacao = new AgregacaoNaoConformidadeMock()
            {
                AcaoPreventivaNaoConformidades = acoesPreventivas,
                CausaNaoConformidades = causas,
                DefeitoNaoConformidades = defeitosNaoConformidadeOutput,
                SolucaoNaoConformidades = solucoesNaoConformidadeOutput,
                CentroCustoCausaNaoConformidades = centrosCustoNaoConformidadeOutputs,
                NaoConformidade = GetNaoConformidadeOutput(0),
                ConclusaoNaoConformidade = GetConclusaoNaoConformidadeOutput(0),
                ReclamacaoNaoConformidade = GetReclamacaoNaoConformidadeOutput(0),
                ImplementacoesEvitarReincidenciaNaoConformidades = implementacoes
            };
            return agregacao;
        }
        public static List<Guid> Guids => new()
        {
            Guid.Parse("FD453364-5BF2-45BB-9402-01CB9108A2D7"),
            Guid.Parse("DAB2569B-1135-4847-8EBC-068EBC80273A"),
            Guid.Parse("A84CD0C9-D8B3-457E-9187-6077C7352792"),
            Guid.Parse("F06A1DD3-3728-4929-9E73-07529F85A0D0"),
            Guid.Parse("CD1FDFB1-AA0B-4CD4-926A-2FDF8D4B7D13"),
            Guid.Parse("6D068495-A380-4B43-81C3-5B6035B57356"),
            Guid.Parse("065468B4-1189-4BF5-9582-22C410F2BD53"),
            Guid.Parse("A81982D7-7BA3-4915-AFD3-8FE69BBE79BA")
        };

        public static List<decimal> Decimals => new()
        {
            new decimal(10.7),
            new decimal(20.6),
            new decimal(30.5),
            new decimal(40.4),
            new decimal(50.3),
            new decimal(60.2),
            new decimal(70.1),
            new decimal(800.3),
            new decimal(900.2),
            new decimal(1000.1),
            new decimal(20500.1),
            new decimal(151000.1),
            new decimal(223390.1),
            new decimal(212.1),
        };

        public static List<string> Strings => new()
        {
            "Harry Potter",
            "O senhor dos aneis",
            "Crepusculo",
            "O nome do Vento",
            "A curandeira da Prisão",
            "De Volta Para o Futuro",
            "Top Gun",
            "Hora do Rush"
        };

        public static List<DateTime> Datas => new()
        {
            new DateTime(2000, 08, 30, 12, 00, 00),
            new DateTime(2000, 09, 30, 12, 00, 00),
            new DateTime(2000, 10, 30, 12, 00, 00),
            new DateTime(2000, 11, 30, 12, 00, 00),
            new DateTime(2000, 12, 30, 12, 00, 00),
        };

        public static List<int> Ints => new()
        {
            1, 2, 3, 4, 5, 6, 7, 8
        };

        public static ICurrentTenant GetCurrentTenant()
        {
            var currentTenant = Substitute.For<ICurrentTenant>();
            currentTenant.Id.Returns(Guids[0]);
            return currentTenant;
        }

        public static ICurrentEnvironment GetCurrentEnvironment()
        {
            var currentEnvironment = Substitute.For<ICurrentEnvironment>();
            currentEnvironment.Id.Returns(Guids[0]);
            return currentEnvironment;
        }
        public static ICurrentCompany GetCurrentCompany()
        {
            var currentCompany = Substitute.For<ICurrentCompany>();
            currentCompany.Id.Returns(Guids[0]);
            currentCompany.LegacyId.Returns(Ints[0]);
            return currentCompany;
        }

        public static UserPreferences GetUserPreferences(int index)
        {
            var userPreferences = new UserPreferences
            {
                UserLocale = Strings[index],
                DefaultUserTimeZone = "America/Sao_Paulo",
                EmailSignature = Strings[index]
            };
            return userPreferences;
        }
        
        public static CompanyDetails GetEmpresa(int index)
        {
            var empresa = new CompanyDetails
            {
                Id = Guids[index],
                LegacyCompanyId = 1,
                Cnpj = Strings[index],
                CompanyName = Strings[index],
                TradingName = Strings[index],
            };
            return empresa;
        }

        public static ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput
            GetImplementacaoEvitarReincidenciaNaoConformidadeViewOutput(int index)
        {
            var implementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],
                Descricao = Strings[index],
                IdResponsavel = Guids[index],
                Responsavel = Strings[index],
                IdAuditor = Guids[index],
                Auditor = Strings[index],
                DataVerificacao = Datas[index],
                DataAnalise = Datas[index],
                AcaoImplementada = false,
                DataPrevistaImplantacao = Datas[index],
                NovaData = Datas[index],
                CompanyId = Guids[index],


            };
            return implementacaoEvitarReincidenciaNaoConformidade;
        }

        public static NaoConformidadeViewOutput GetNaoConformidadeViewOutput(int index)
        {
            var naoConformidade = new NaoConformidadeViewOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                Codigo = Ints[index],
                Origem = OrigemNaoConformidade.Cliente,
                Status = StatusNaoConformidade.Aberto,
                IdNotaFiscal = Guids[index],
                NumeroNotaFiscal = Ints[index]
                    .ToString(),
                IdNatureza = Guids[index],
                DescricaoNatureza = Strings[index],
                CodigoNatureza = Ints[index],
                Natureza = Strings[index],
                IdCliente = Guids[index],
                DescricaoCliente = Strings[index],
                CodigoCliente = Ints[index].ToString(),
                Cliente = Strings[index],
                IdFornecedor = Guids[index],
                DescricaoFornecedor = Strings[index],
                CodigoFornecedor = Ints[index].ToString(),
                Fornecedor = Strings[index],
                NumeroOdf = Ints[index],
                IdProduto = Guids[index],
                DescricaoProduto = Strings[index],
                CodigoProduto = Ints[index].ToString(),
                Produto = Strings[index],
                IdLote = Guids[index],
                NumeroLote = Ints[index]
                    .ToString(),
                DataFabricacaoLote = Datas[index],
                CampoNf = Strings[index],
                IdCriador = Guids[index],
                Revisao = Ints[index]
                    .ToString(),
                LoteTotal = false,
                LoteParcial = false,
                Rejeitado = false,
                AceitoConcessao = false,
                RetrabalhoPeloCliente = false,
                RetrabalhoNoCliente = false,
                Equipe = Strings[index],
                NaoConformidadeEmPotencial = false,
                RelatoNaoConformidade = false,
                MelhoriaEmPotencial = false,
                Descricao = Strings[index],
                NomeUsuarioCriador = Strings[index],
                SobrenomeUsuarioCriador = Strings[index],



            };
            return naoConformidade;
        }

        public static AcaoPreventivaNaoConformidadeViewOutput GetAcaoPreventivaNaoConformidadeViewOutput(int index)
        {
            var acaoPreventivaNaoConformidade = new AcaoPreventivaNaoConformidadeViewOutput
            {
                Acao = Strings[index],
                Responsavel = Strings[index],
                Auditor = Strings[index],
                Codigo = Ints[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                IdAcaoPreventiva = Guids[index],
                Id = Guids[index],
                Implementada = false,
                DataAnalise = Datas[index],
                DataVerificacao = Datas[index],
                IdAuditor = Guids[index],
                IdResponsavel = Guids[index],
                NovaData = Datas[index],
                DataPrevistaImplantacao = Datas[index],
                IdNaoConformidade = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],

            };
            return acaoPreventivaNaoConformidade;
        }

        public static CausaNaoConformidadeViewOutput GetCausaNaoConformidadeViewOutput(int index)
        {
            var causaNaoConformidade = new CausaNaoConformidadeViewOutput
            {
                Id = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],
                Detalhamento = Strings[index],
                IdCausa = Guids[index],
                Codigo = Ints[index],
                Descricao = Strings[index],
                IdNaoConformidade = Guids[index],

            };
            return causaNaoConformidade;
        }

        public static DefeitoNaoConformidadeViewOutput GetDefeitoNaoConformidadeViewOutput(int index)
        {
            var defeito = new DefeitoNaoConformidadeViewOutput
            {
                Id = Guids[index],
                Detalhamento = Strings[index],
                Descricao = Strings[index],
                Codigo = Ints[index],
                IdDefeito = Guids[index],
                Quantidade = Ints[index],
                IdNaoConformidade = Guids[index],

            };
            return defeito;
        }

        public static SolucaoNaoConformidadeViewOutput GetSolucaoNaoConformidadeViewOutput(int index)
        {
            var solucaoNaoConformidade = new SolucaoNaoConformidadeViewOutput
            {
                Id = Guids[index],
                IdSolucao = Guids[index],
                Codigo = Ints[index],
                Descricao = Strings[index],
                DataVerificacao = Datas[index],
                CustoEstimado = Decimals[index],
                IdNaoConformidade = Guids[index],
                IdAuditor = Guids[index],
                Responsavel = Strings[index],
                Auditor = Strings[index],
                DataAnalise = Datas[index],
                IdResponsavel = Guids[index],
                NovaData = Datas[index],
                SolucaoImediata = false,
                DataPrevistaImplantacao = Datas[index],
                IdDefeitoNaoConformidade = Guids[index],
                Detalhamento = Strings[index],
            };
            return solucaoNaoConformidade;
        }

        public static CentroCustoCausaNaoConformidadeViewOutput GetCentroCustoCausaNaoConformidadeViewOutput(int index)
        {
            var centroCusto = new CentroCustoCausaNaoConformidadeViewOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                IdCausaNaoConformidade = Guids[index],
                CodigoCentroCusto = Ints[index].ToString(),
                DescricaoCentroCusto = Strings[index],
                IsCentroCustoSintetico = false,
            };
            return centroCusto;
        }

        public static ImplementacaoEvitarReincidenciaNaoConformidadeOutput
            GetImplementacaoEvitarReincidenciaNaoConformidadeDto(int index)
        {
            var implementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaNaoConformidadeOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],
                Descricao = Strings[index],
                IdResponsavel = Guids[index],
                IdAuditor = Guids[index],
                DataVerificacao = Datas[index],
                DataAnalise = Datas[index],
                AcaoImplementada = false,
                DataPrevistaImplantacao = Datas[index],
                NovaData = Datas[index],


            };
            return implementacaoEvitarReincidenciaNaoConformidade;
        }

        public static NaoConformidadeOutput GetNaoConformidadeOutput(int index)
        {
            var naoConformidade = new NaoConformidadeOutput
            {
                Id = Guids[index],
                Codigo = Ints[index],
                Origem = OrigemNaoConformidade.Cliente,
                Status = StatusNaoConformidade.Aberto,
                IdNotaFiscal = Guids[index],
                NumeroNotaFiscal = Ints[index]
                    .ToString(),
                IdNatureza = Guids[index],
                IdPessoa = Guids[index],
                NumeroOdf = Ints[index],
                IdProduto = Guids[index],
                IdLote = Guids[index],
                NumeroLote = Ints[index]
                    .ToString(),
                NumeroOdfRetrabalho = Ints[index],
                IdOdfRetrabalho = Guids[index],
                NumeroPedido = Ints[index].ToString(),
                NumeroOdfFaturamento = Ints[index],
                IdProdutoFaturamento = Guids[index],
                Incompleta = false,
                DataCriacao = Datas[index],
                CompanyId = default,
                DataFabricacaoLote = Datas[index],
                CampoNf = Strings[index],
                IdCriador = Guids[index],
                Revisao = Ints[index]
                    .ToString(),
                LoteTotal = false,
                LoteParcial = false,
                Rejeitado = false,
                AceitoConcessao = false,
                RetrabalhoPeloCliente = false,
                RetrabalhoNoCliente = false,
                Equipe = Strings[index],
                NaoConformidadeEmPotencial = false,
                RelatoNaoConformidade = false,
                MelhoriaEmPotencial = false,
                Descricao = Strings[index],

            };
            return naoConformidade;
        }

        public static AcaoPreventivaNaoConformidadeOutput GetAcaoPreventivaNaoConformidadeOutput(int index)
        {
            var acaoPreventivaNaoConformidade = new AcaoPreventivaNaoConformidadeOutput
            {
                Acao = Strings[index],
                Detalhamento = Strings[index],
                IdAcaoPreventiva = Guids[index],
                Id = Guids[index],
                Implementada = false,
                DataAnalise = Datas[index],
                DataVerificacao = Datas[index],
                IdAuditor = Guids[index],
                IdResponsavel = Guids[index],
                NovaData = Datas[index],
                DataPrevistaImplantacao = Datas[index],
                IdNaoConformidade = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],

            };
            return acaoPreventivaNaoConformidade;
        }

        public static CausaNaoConformidadeOutput GetCausaNaoConformidadeOutput(int index)
        {
            var causaNaoConformidade = new CausaNaoConformidadeOutput
            {
                Id = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],
                Detalhamento = Strings[index],
                IdCausa = Guids[index],
                IdNaoConformidade = Guids[index],

            };
            return causaNaoConformidade;
        }

        public static DefeitoNaoConformidadeOutput GetDefeitoNaoConformidadeOutput(int index)
        {
            var defeito = new DefeitoNaoConformidadeOutput
            {
                Id = Guids[index],
                Detalhamento = Strings[index],
                IdDefeito = Guids[index],
                Quantidade = Ints[index],
                IdNaoConformidade = Guids[index],

            };
            return defeito;
        }

        public static SolucaoNaoConformidadeOutput GetSolucaoNaoConformidadeOutput(int index)
        {
            var solucaoNaoConformidade = new SolucaoNaoConformidadeOutput
            {
                Id = Guids[index],
                IdSolucao = Guids[index],
                DataVerificacao = Datas[index],
                CustoEstimado = Decimals[index],
                IdNaoConformidade = Guids[index],
                IdAuditor = Guids[index],
                DataAnalise = Datas[index],
                IdResponsavel = Guids[index],
                NovaData = Datas[index],
                SolucaoImediata = false,
                DataPrevistaImplantacao = Datas[index],
                IdDefeitoNaoConformidade = Guids[index],
                Detalhamento = Strings[index],
            };
            return solucaoNaoConformidade;
        }

        public static CentroCustoCausaNaoConformidadeOutput GetCentroCustoCausaNaoConformidadeDto(int index)
        {
            var centroCusto = new CentroCustoCausaNaoConformidadeOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                IdCausaNaoConformidade = Guids[index]
            };
            return centroCusto;
        }
        public static ConclusaoNaoConformidadeOutput GetConclusaoNaoConformidadeOutput(int index)
        {
            var conclusaoNaoConformidade = new ConclusaoNaoConformidadeOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                NovaReuniao = false,
                DataReuniao = Datas[index],
                DataVerificacao = Datas[index],
                IdAuditor = Guids[index],
                Evidencia = Strings[index],
                Eficaz = false,
                CicloDeTempo = Ints[index],
                IdNovoRelatorio = Guids[index],

            };
            return conclusaoNaoConformidade;
        }

        public static ReclamacaoNaoConformidadeOutput GetReclamacaoNaoConformidadeOutput(int index)
        {
            var reclamacaoNaoConformidade = new ReclamacaoNaoConformidadeOutput
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                Procedentes = Ints[index],
                Improcedentes = Ints[index],
                QuantidadeLote = Ints[index],
                QuantidadeNaoConformidade = Ints[index],
                DisposicaoProdutosAprovados = Ints[index],
                DisposicaoProdutosConcessao = Ints[index],
                Retrabalho = Ints[index],
                Rejeitado = Ints[index],
                RetrabalhoComOnus = false,
                RetrabalhoSemOnus = false,
                DevolucaoFornecedor = false,
                Recodificar = false,
                Sucata = false,
                Observacao = Strings[index],
            };
            return reclamacaoNaoConformidade;
        }

        public static OperacaoRetrabalhoNaoConformidade GetOperacaoRetrabalhoNaoConformidade(int index)
        {
            var operacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidade
            {
                Id = Guids[index],
                IdNaoConformidade = Guids[index],
                Quantidade = Ints[index],
                NumeroOperacaoARetrabalhar = Ints[index].ToString(),
            };
            return operacaoRetrabalhoNaoConformidade;
        }

        public static OperacaoViewOutput GetOperacaoViewOutput(int index)
        {
            var operacaoViewOutput = new OperacaoViewOutput
            {
                Id = Guids[index],
                NumeroOperacao = Ints[index].ToString(),
                IdRecurso = Guids[index],
                DescricaoRecurso = Strings[index],
                CodigoRecurso = Ints[index].ToString(),
                IdOperacaoRetrabalhoNaoConformdiade = Guids[index],
                Status = StatusProducaoRetrabalho.Aberta
            };
            return operacaoViewOutput;
        }
    }
}
