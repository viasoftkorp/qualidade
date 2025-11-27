using System;
using System.Collections.Generic;
using FluentAssertions.Equivalency;
using NSubstitute;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.DDD.Entities.Auditing;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.Testing;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Infrastructure.EntityFrameworkCore;


namespace Viasoft.Qualidade.RNC.Core.UnitTest;

public class TestUtils
{
    public class UnitTestBaseWithDbContext : UnitTestBase
    {
        protected override Type GetDbContextType()
        {
            return typeof(ViasoftQualidadeRNCCoreDbContext);
        }
    }
    public class ObjectMother
    {
        public class AgregacaoNaoConformidadeMock
        {
            public AgregacaoNaoConformidade AgregacaoFromThis()
            {
                return new AgregacaoNaoConformidade(NaoConformidade, AcaoPreventivaNaoConformidades,
                    CausaNaoConformidades, DefeitoNaoConformidades, SolucaoNaoConformidades,
                    ProdutoNaoConformidades, ServicoNaoConformidades, CentroCustoCausaNaoConformidades,
                    ConclusaoNaoConformidade, ReclamacaoNaoConformidade, OrdemRetrabalhoNaoConformidade, ImplementacoesEvitarReincidenciaNaoConformidades);
            }

            public NaoConformidade NaoConformidade { get; set; }
            public ConclusaoNaoConformidade ConclusaoNaoConformidade { get; set; }
            public ReclamacaoNaoConformidade ReclamacaoNaoConformidade { get; set; }
            public OrdemRetrabalhoNaoConformidade OrdemRetrabalhoNaoConformidade { get; set; }
            public List<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidades { get; set; }
            public List<CausaNaoConformidade> CausaNaoConformidades { get; set; }
            public List<DefeitoNaoConformidade> DefeitoNaoConformidades { get; set; }
            public List<SolucaoNaoConformidade> SolucaoNaoConformidades { get; set; }
            public List<ProdutoNaoConformidade> ProdutoNaoConformidades { get; set; }
            public List<ServicoNaoConformidade> ServicoNaoConformidades { get; set; }
            public List<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacoesEvitarReincidenciaNaoConformidades { get; set; }
            public List<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidades { get; set; }

            public AgregacaoNaoConformidadeMock()
            {
                AcaoPreventivaNaoConformidades = new List<AcaoPreventivaNaoConformidade>();
                CausaNaoConformidades = new List<CausaNaoConformidade>();
                DefeitoNaoConformidades = new List<DefeitoNaoConformidade>();
                SolucaoNaoConformidades = new List<SolucaoNaoConformidade>();
                ProdutoNaoConformidades = new List<ProdutoNaoConformidade>();
                ServicoNaoConformidades = new List<ServicoNaoConformidade>();
                ImplementacoesEvitarReincidenciaNaoConformidades =
                    new List<ImplementacaoEvitarReincidenciaNaoConformidade>();
            }
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

        public static Causa GetCausa(int index)
        {
            var causa = new Causa
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                Codigo = Ints[index],
                IsAtivo = true
            };
            return causa;
        }

        public static AcaoPreventiva GetAcaoPreventiva(int index)
        {
            var acaoPreventiva = new AcaoPreventiva
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                Codigo = Ints[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                IdResponsavel = Guids[index],
                IsAtivo = true
            };
            return acaoPreventiva;
        }

        public static Natureza GetNatureza(int index)
        {
            var natureza = new Natureza
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                Descricao = Strings[index],
                Codigo = Ints[index],
                IsAtivo = true
            };
            return natureza;
        }
        public static NaturezaInput GetNaturezaInput(int index)
        {
            var naturezaInput = new NaturezaInput
            {
                Id = Guids[index],
                Descricao = Strings[index],
                Codigo = Ints[index],
                IsAtivo = true
            };
            return naturezaInput;
        }

        public static Solucao GetSolucao(int index)
        {
            var solucao = new Solucao
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                Imediata = false,
                Codigo = Ints[index],
                IsAtivo = false
            };
            return solucao;
        }
        public static SolucaoInput GetSolucaoInput(int index)
        {
            var solucaoInput = new SolucaoInput
            {
                Id = Guids[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                Imediata = false,
                Codigo = Ints[index],
                IsAtivo = false
            };
            return solucaoInput;
        }

        public static ProdutoSolucao GetProdutoSolucao(int index)
        {
            var produtoSolucao = new ProdutoSolucao
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                IdSolucao = Guids[index],
                IdProduto = Guids[index],
                Quantidade = Ints[index],
                OperacaoEngenharia = Strings[index]
            };
            return produtoSolucao;
        }
        public static ProdutoSolucaoInput GetProdutoSolucaoInput(int index)
        {
            var produtoSolucaoInput = new ProdutoSolucaoInput
            {
                Id = Guids[index],
                IdSolucao = Guids[index],
                IdProduto = Guids[index],
                Quantidade = Ints[index],
                OperacaoEngenharia = Strings[index]
            };
            return produtoSolucaoInput;
        }

        public static ServicoSolucao GetServicoSolucao(int index)
        {
            var servicoSolucao = new ServicoSolucao
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                IdSolucao = Guids[index],
                IdProduto = Guids[index],
                Quantidade = Ints[index],
                Horas = Ints[index],
                Minutos = Ints[index],
                IdRecurso = Guids[index],
                OperacaoEngenharia = Strings[index]
            };
            return servicoSolucao;
        }
        public static ServicoSolucaoInput GetServicoSolucaoInput(int index)
        {
            var servicoSolucaoInput = new ServicoSolucaoInput
            {
                Id = Guids[index],
                IdSolucao = Guids[index],
                IdProduto = Guids[index],
                Quantidade = Ints[index],
                Horas = Ints[index],
                Minutos = Ints[index],
                IdRecurso = Guids[index],
                OperacaoEngenharia = Strings[index]
            };
            return servicoSolucaoInput;
        }

        public static Defeito GetDefeito(int index)
        {
            var defeito = new Defeito
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                Codigo = Ints[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                IdCausa = Guids[index],
                IdSolucao = Guids[index],
                IsAtivo = true
            };
            return defeito;
        }
        public static DefeitoInput GetDefeitoInput(int index)
        {
            var defeitoInput = new DefeitoInput
            {
                Id = Guids[index],
                Codigo = Ints[index],
                Descricao = Strings[index],
                Detalhamento = Strings[index],
                IdCausa = Guids[index],
                IdSolucao = Guids[index],
                IsAtivo = true
            };
            return defeitoInput;
        }
        public static Produto GetProduto(int index)
        {
            var produto = new Produto
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Descricao = Strings[index],
                IdUnidadeMedida = Guids[index],
                IdCategoria = Guids[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                Codigo = Ints[index].ToString(),
            };
            return produto;
        }

        public static UnidadeMedidaProduto GetUnidadeMedidaProduto(int index)
        {
            var unidadeMedidaProduto = new UnidadeMedidaProduto
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Descricao = Strings[index],
                Codigo = Ints[index].ToString(),
                EnvironmentId = Guids[index],
                TenantId = Guids[index]
            };
            return unidadeMedidaProduto;
        }

        public static Recurso GetRecurso(int index)
        {
            var recurso = new Recurso
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Descricao = Strings[index],
                Codigo = Ints[index].ToString(),
                EnvironmentId = Guids[index],
                TenantId = Guids[index]
            };
            return recurso;
        }

        public static Cliente GetCliente(int index)
        {
            var cliente = new Cliente
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Codigo = Ints[index].ToString(),
                RazaoSocial = Strings[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],

            };
            return cliente;
        }

        public static Usuario GetUsuario(int index)
        {
            var usuario = new Usuario
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Nome = Strings[index],
                Sobrenome = Strings[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index]
            };
            return usuario;
        }

        public static AgregacaoNaoConformidadeMock GetAgregacaoNaoConformidadeMock(int index)
        {
            var acoesPreventivas = new List<AcaoPreventivaNaoConformidade>
            {
                GetAcaoPreventivaNaoConformidade(index)
            };
            var causas = new List<CausaNaoConformidade>
            {
                GetCausaNaoConformidade(index)
            };
            var defeitosNaoConformidade = new List<DefeitoNaoConformidade>
            {
                GetDefeitoNaoConformidade(index)
            };
            var solucoesNaoConformidade = new List<SolucaoNaoConformidade>
            {
                GetSolucaoNaoConformidade(index)
            };
            var produtosNaoConformidade = new List<ProdutoNaoConformidade>
            {
                GetProdutoNaoConformidade(index)
            };
            var servicosNaoConformidades = new List<ServicoNaoConformidade>
            {
                GetServicoNaoConformidade(index)
            };
            var centrosCustoNaoConformidades = new List<CentroCustoCausaNaoConformidade>
            {
                GetCentroCustoCausaNaoConformidade(index)
            };
            var implementacoes = new List<ImplementacaoEvitarReincidenciaNaoConformidade>
            {
                GetImplementacaoEvitarReincidenciaNaoConformidade(index)
            };
            var agregacao = new AgregacaoNaoConformidadeMock()
            {
                AcaoPreventivaNaoConformidades = acoesPreventivas,
                CausaNaoConformidades = causas,
                DefeitoNaoConformidades = defeitosNaoConformidade,
                SolucaoNaoConformidades = solucoesNaoConformidade,
                ProdutoNaoConformidades = produtosNaoConformidade,
                ServicoNaoConformidades = servicosNaoConformidades,
                CentroCustoCausaNaoConformidades = centrosCustoNaoConformidades,
                NaoConformidade = GetNaoConformidade(index),
                ConclusaoNaoConformidade = GetConclusaoNaoConformidade(index),
                ReclamacaoNaoConformidade = GetReclamacaoNaoConformidade(index),
                OrdemRetrabalhoNaoConformidade = GetOrdemRetrabalhoNaoConfrmidade(index),
                ImplementacoesEvitarReincidenciaNaoConformidades = implementacoes
            };
            return agregacao;
        }

        public static OrdemRetrabalhoNaoConformidade GetOrdemRetrabalhoNaoConfrmidade(int index)
        {
            var ordemRetrabalho = new OrdemRetrabalhoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                IdNaoConformidade = Guids[index],
                NumeroOdfRetrabalho = Ints[index],
                Quantidade = Ints[index],
                IdLocalOrigem = Guids[index],
                IdEstoqueLocalDestino = Guids[index],
                IdLocalDestino = Guids[index],
                MovimentacaoEstoqueMensagemRetorno = null,
                CodigoArmazem = Ints[index].ToString(),
                DataFabricacao = Datas[index],
                DataValidade = Datas[index],
                Status = StatusProducaoRetrabalho.Aberta
            };
            return ordemRetrabalho;
        }

        public static ConclusaoNaoConformidade GetConclusaoNaoConformidade(int index)
        {
            var conclusaoNaoConformidade = new ConclusaoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                IdNaoConformidade = Guids[index],
                NovaReuniao = false,
                DataReuniao = Datas[index],
                DataVerificacao = Datas[index],
                IdAuditor = Guids[index],
                Evidencia = Strings[index],
                Eficaz = false,
                CicloDeTempo = Ints[index],
                IdNovoRelatorio = Guids[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                CompanyId = Guids[index],

            };
            return conclusaoNaoConformidade;
        }

        public static ReclamacaoNaoConformidade GetReclamacaoNaoConformidade(int index)
        {
            var reclamacaoNaoConformidade = new ReclamacaoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
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
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                CompanyId = Guids[index],

            };
            return reclamacaoNaoConformidade;
        }

        public static ImplementacaoEvitarReincidenciaNaoConformidade
            GetImplementacaoEvitarReincidenciaNaoConformidade(int index)
        {
            var implementacaoEvitarReincidenciaNaoConformidade = new ImplementacaoEvitarReincidenciaNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                IdNaoConformidade = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],
                Descricao = Strings[index],
                IdResponsavel = Guids[index],
                IdAuditor = Guids[index],
                DataVerificacao = Datas[index],
                DataAnalise = Datas[index],
                AcaoImplementada = false,
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                DataPrevistaImplantacao = Datas[index],
                NovaData = Datas[index],
                CompanyId = Guids[index],

            };
            return implementacaoEvitarReincidenciaNaoConformidade;
        }

        public static NaoConformidade GetNaoConformidade(int index)
        {
            var naoConformidade = new NaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = Datas[index],
                LastModifierId = Guids[index],
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
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
                DataFabricacaoLote = Datas[index],
                CampoNf = Strings[index],
                IdCriador = Guids[index],
                DataCriacao = Datas[index],
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
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                CompanyId = Guids[index],
                NumeroPedido = Strings[index],
                NumeroOdfFaturamento = Ints[index],
                IdProdutoFaturamento = Guids[index],
                Incompleta = false,
                OperacaoRetrabalho = null,

            };
            return naoConformidade;
        }

        public static AcaoPreventivaNaoConformidade GetAcaoPreventivaNaoConformidade(int index)
        {
            var acaoPreventivaNaoConformidade = new AcaoPreventivaNaoConformidade
            {
                Acao = Strings[index],
                Detalhamento = Strings[index],
                IdAcaoPreventiva = Guids[index],
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Implementada = false,
                DataAnalise = Datas[index],
                DataVerificacao = Datas[index],
                IdAuditor = Guids[index],
                IdResponsavel = Guids[index],
                NovaData = Datas[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                DataPrevistaImplantacao = Datas[index],
                IdNaoConformidade = Guids[index],
                IdDefeitoNaoConformidade = Guids[index],
                CompanyId = Guids[index]
            };
            return acaoPreventivaNaoConformidade;
        }

        public static CausaNaoConformidade GetCausaNaoConformidade(int index)
        {
            var causaNaoConformidade = new CausaNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                IdDefeitoNaoConformidade = Guids[index],
                Detalhamento = Strings[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                IdCausa = Guids[index],
                IdNaoConformidade = Guids[index],
                CompanyId = Guids[index]
            };
            return causaNaoConformidade;
        }

        public static DefeitoNaoConformidade GetDefeitoNaoConformidade(int index)
        {
            var defeito = new DefeitoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                Detalhamento = Strings[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                IdDefeito = Guids[index],
                Quantidade = Ints[index],
                IdNaoConformidade = Guids[index],
                CompanyId = Guids[index],
            };
            return defeito;
        }

        public static SolucaoNaoConformidade GetSolucaoNaoConformidade(int index)
        {
            var solucaoNaoConformidade = new SolucaoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
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
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                CompanyId = Guids[index],

            };
            return solucaoNaoConformidade;
        }

        public static ProdutoNaoConformidade GetProdutoNaoConformidade(int index)
        {
            var produtoNaoConformidade = new ProdutoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                IdProduto = Guids[index],
                IdNaoConformidade = Guids[index],
                Quantidade = Ints[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                Detalhamento = Strings[index],
                CompanyId = Guids[index],
                OperacaoEngenharia = Strings[index],

            };
            return produtoNaoConformidade;
        }

        public static ServicoNaoConformidade GetServicoNaoConformidade(int index)
        {
            var servicoNaoConformidade = new ServicoNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                IdProduto = Guids[index],
                IdNaoConformidade = Guids[index],
                Quantidade = Ints[index],
                Horas = Ints[index],
                Minutos = Ints[index],
                IdRecurso = Guids[index],
                OperacaoEngenharia = Strings[index],
                EnvironmentId = Guids[index],
                TenantId = Guids[index],
                Detalhamento = Strings[index],
                CompanyId = Guids[index],
            };
            return servicoNaoConformidade;
        }
        public static CentroCustoCausaNaoConformidade GetCentroCustoCausaNaoConformidade(int index)
        {
            var centroCusto = new CentroCustoCausaNaoConformidade
            {
                Id = Guids[index],
                CreationTime = Datas[index],
                CreatorId = Guids[index],
                LastModificationTime = null,
                LastModifierId = null,
                DeleterId = null,
                DeletionTime = null,
                IsDeleted = false,
                IdNaoConformidade = Guids[index],
                IdCausaNaoConformidade = Guids[index],
                TenantId = Guids[index],
                EnvironmentId = Guids[index],
                CompanyId = Guids[index],
                IdCentroCusto = Guids[index],

            };
            return centroCusto;
        }
    }
    public static EquivalencyAssertionOptions<T> ExcludeAuditoria<T>(EquivalencyAssertionOptions<T> options) where T : FullAuditedEntity
    {
        return options.Excluding(e => e.CreationTime)
            .Excluding(e => e.CreatorId)
            .Excluding(e => e.LastModificationTime)
            .Excluding(e => e.LastModifierId)
            .Excluding(e => e.DeletionTime)
            .Excluding(e => e.DeleterId);
    }

    public static void LimparTracker<T>(IRepository<T> repository) where T : class
    {
        repository.GetUnderlyingDbContext().ChangeTracker.Clear();
    }
}
