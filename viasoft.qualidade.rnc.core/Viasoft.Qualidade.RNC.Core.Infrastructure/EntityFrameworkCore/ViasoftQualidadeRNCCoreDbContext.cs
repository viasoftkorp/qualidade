using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Viasoft.Core.EntityFrameworkCore.Context;
using Viasoft.Core.Storage.Schema;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.CentroCustos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.PedidoVendas;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.EntityFrameworkCore
{
    public class ViasoftQualidadeRNCCoreDbContext : BaseDbContext
    {
        private StringToBytesConverter StringToByteArrayConverter = new StringToBytesConverter(Encoding.UTF8);

        public DbSet<Natureza> Naturezas { get; set; }
        public DbSet<Causa> Causas { get; set; }
        public DbSet<Solucao> Solucoes { get; set; }
        public DbSet<Defeito> Defeitos { get; set; }
        public DbSet<AcaoPreventiva> AcoesPreventivas { get; set; }
        public DbSet<ProdutoSolucao> ProdutoSolucoes { get; set; }
        public DbSet<ServicoSolucao> ServicoSolucoes { get; set; }
        public DbSet<NaoConformidade> NaoConformidades { get; set; }
        public DbSet<DefeitoNaoConformidade> DefeitoNaoConformidades { get; set; }
        public DbSet<CausaNaoConformidade> CausaNaoConformidades { get; set; }
        public DbSet<SolucaoNaoConformidade> SolucaoNaoConformidades { get; set; }
        public DbSet<ProdutoNaoConformidade> ProdutoNaoConformidades { get; set; }
        public DbSet<ServicoNaoConformidade> ServicoNaoConformidades { get; set; }
        public DbSet<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidades { get; set; }
        public DbSet<ReclamacaoNaoConformidade> ReclamacaoClientes { get; set; }
        public DbSet<ConclusaoNaoConformidade> ConclusaoNaoConformidades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<CentroCusto> CentrosCustos { get; set; }
        public DbSet<UnidadeMedidaProduto> UnidadeMedidaProdutos { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidades { get; set; }
        public DbSet<SeederManager> SeederManagers { get; set; }
        public DbSet<ConfiguracaoGeral> ConfiguracoesGerais { get; set; }
        public DbSet<ImplementacaoEvitarReincidenciaNaoConformidade> ImplementacaoEvitarReincidenciaNaoConformidades { get; set; }
        public DbSet<CentroCustoCausaNaoConformidade> CentroCustoCausaNaoConformidades { get; set; }
        public DbSet<PedidoVenda> PedidoVendas { get; set; }
        public DbSet<OperacaoRetrabalhoNaoConformidade> OperacaoRetrabalhoNaoConformidades { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Local> Locais { get; set; }


        public ViasoftQualidadeRNCCoreDbContext(DbContextOptions options, ISchemaNameProvider schemaNameProvider,
            ILoggerFactory loggerFactory,
            IBaseDbContextConfigurationService baseDbContextConfigurationService) : base(options, schemaNameProvider,
            loggerFactory, baseDbContextConfigurationService)
        {
        }
        private static string IsDeleted => "\"isdeleted\" = false";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            NaturezaModel(modelBuilder);

            CausaModel(modelBuilder);

            DefeitoModel(modelBuilder);

            AcaoPreventivaModel(modelBuilder);

            SolucaoModel(modelBuilder);

            var produtoSolucao = modelBuilder.Entity<ProdutoSolucao>();
            produtoSolucao.ToTable(nameof(ProdutoSolucao).ToLower());
            produtoSolucao.Property(e => e.OperacaoEngenharia)
                .HasConversion(StringToByteArrayConverter);
            produtoSolucao.Property(e => e.OperacaoEngenharia).Metadata.SetMaxLength(null);

            var servicoSolucao = modelBuilder.Entity<ServicoSolucao>();
            servicoSolucao.Property(e => e.OperacaoEngenharia)
                .HasConversion(StringToByteArrayConverter);
            servicoSolucao.Property(e => e.OperacaoEngenharia).Metadata.SetMaxLength(null);
            servicoSolucao.ToTable(nameof(ServicoSolucao).ToLower());

            var naoConformidade = modelBuilder.Entity<NaoConformidade>();
            naoConformidade.HasIndex(e => e.Codigo)
                .HasFilter(IsDeleted)
                .IsUnique();
            naoConformidade.Property(e => e.Descricao)
                .HasConversion(StringToByteArrayConverter);
            naoConformidade.Property(e => e.Descricao).Metadata.SetMaxLength(null);
            naoConformidade.ToTable(nameof(NaoConformidade).ToLower());

            var defeitoNaoConformidade = modelBuilder.Entity<DefeitoNaoConformidade>();
            defeitoNaoConformidade.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            defeitoNaoConformidade.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            defeitoNaoConformidade.ToTable(nameof(DefeitoNaoConformidade).ToLower());

            var causaNaoConformidade = modelBuilder.Entity<CausaNaoConformidade>();
            causaNaoConformidade.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            causaNaoConformidade.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            causaNaoConformidade.ToTable(nameof(CausaNaoConformidade).ToLower());

            var solucaoNaoConformidade = modelBuilder.Entity<SolucaoNaoConformidade>();
            solucaoNaoConformidade.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            solucaoNaoConformidade.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            solucaoNaoConformidade.ToTable(nameof(SolucaoNaoConformidade).ToLower());

            var produtoNaoConformidade = modelBuilder.Entity<ProdutoNaoConformidade>();
            produtoNaoConformidade.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            produtoNaoConformidade.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            produtoNaoConformidade.ToTable(nameof(produtoNaoConformidade).ToLower());
            produtoNaoConformidade.Property(e => e.OperacaoEngenharia)
                .HasConversion(StringToByteArrayConverter);
            produtoNaoConformidade.Property(e => e.OperacaoEngenharia).Metadata.SetMaxLength(null);

            var servicoSolucaoNaoConformidade = modelBuilder.Entity<ServicoNaoConformidade>();
            servicoSolucaoNaoConformidade.Property(e => e.OperacaoEngenharia)
                .HasConversion(StringToByteArrayConverter);
            servicoSolucaoNaoConformidade.Property(e => e.OperacaoEngenharia).Metadata.SetMaxLength(null);
            servicoSolucaoNaoConformidade.Property(e => e.Detalhamento)
                .HasConversion(StringToByteArrayConverter);
            servicoSolucaoNaoConformidade.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            servicoSolucaoNaoConformidade.ToTable(nameof(ServicoNaoConformidade).ToLower());

            var acaoPreventivaNaoConformidade = modelBuilder.Entity<AcaoPreventivaNaoConformidade>();
            acaoPreventivaNaoConformidade.Property(e => e.Detalhamento)
                .HasConversion(StringToByteArrayConverter);
            acaoPreventivaNaoConformidade.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            acaoPreventivaNaoConformidade.ToTable(nameof(AcaoPreventivaNaoConformidade).ToLower());

            var reclamacaoCliente = modelBuilder.Entity<ReclamacaoNaoConformidade>();
            reclamacaoCliente.Property(e => e.Observacao)
                .HasConversion(StringToByteArrayConverter);
            reclamacaoCliente.Property(e => e.Observacao).Metadata.SetMaxLength(null);
            reclamacaoCliente.ToTable(nameof(ReclamacaoNaoConformidade).ToLower());

            var conclusaoNaoConformidade = modelBuilder.Entity<ConclusaoNaoConformidade>();
            conclusaoNaoConformidade.ToTable(nameof(ConclusaoNaoConformidade).ToLower());

            var usuario = modelBuilder.Entity<Usuario>();
            usuario.ToTable(nameof(Usuario).ToLower());

            var produto = modelBuilder.Entity<Produto>();
            produto.ToTable(nameof(Produto).ToLower());

            var unidadeMedidaProduto = modelBuilder.Entity<UnidadeMedidaProduto>();
            unidadeMedidaProduto.ToTable(nameof(UnidadeMedidaProduto).ToLower());

            var recurso = modelBuilder.Entity<Recurso>();
            recurso.ToTable(nameof(Recurso).ToLower());

            var cliente = modelBuilder.Entity<Cliente>();
            cliente.ToTable(nameof(Cliente).ToLower());

            OrdemRetrabalhoNaoConformidadeModel(modelBuilder);
            ConfiguracaoGeralModel(modelBuilder);
            SeederManagerModel(modelBuilder);
            ImplementacaoEvitarReincidenciaModel(modelBuilder);
            PedidoVendaModel(modelBuilder);
            OperacaoRetrabalhoNaoConformidadeModel(modelBuilder);
            OperacaoModel(modelBuilder);
            CentroCustoModel(modelBuilder);
            LocalModel(modelBuilder);
        }
        private void NaturezaModel(ModelBuilder modelBuilder)
        {
            var natureza = modelBuilder.Entity<Natureza>();
            natureza.Property(e => e.Codigo).ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1, minValue: 1);

            natureza.Property(e => e.IsAtivo)
                .HasDefaultValue(true);

            natureza.ToTable(nameof(Natureza).ToLower());
        }
        private void CausaModel(ModelBuilder modelBuilder)
        {
            var causa = modelBuilder.Entity<Causa>();
            causa.Property(e => e.Codigo).ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1, minValue: 1);
            causa.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            causa.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            causa.Property(e => e.IsAtivo).HasDefaultValue(true);
            causa.ToTable(nameof(Causa).ToLower());
        }
        private void SolucaoModel(ModelBuilder modelBuilder)
        {
            var solucao = modelBuilder.Entity<Solucao>();
            solucao.Property(e => e.Codigo).ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1, minValue: 1);
            solucao.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            solucao.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            solucao.Property(e => e.IsAtivo).HasDefaultValue(true);
            solucao.ToTable(nameof(Solucao).ToLower());
        }
        private void DefeitoModel(ModelBuilder modelBuilder)
        {
            var defeito = modelBuilder.Entity<Defeito>();
            defeito.Property(e => e.Codigo).ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1, minValue: 1);
            defeito.Property(e => e.Detalhamento).HasConversion(StringToByteArrayConverter);
            defeito.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            defeito.Property(e => e.IsAtivo).HasDefaultValue(true);
            defeito.ToTable(nameof(Defeito).ToLower());
        }

        private void AcaoPreventivaModel(ModelBuilder modelBuilder)
        {
            var acaoPreventiva = modelBuilder.Entity<AcaoPreventiva>();
            acaoPreventiva.Property(e => e.Codigo).ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1, minValue: 1);
            acaoPreventiva.Property(e => e.Detalhamento)
                .HasConversion(StringToByteArrayConverter);
            acaoPreventiva.Property(e => e.Detalhamento).Metadata.SetMaxLength(null);
            acaoPreventiva.Property(e => e.IsAtivo).HasDefaultValue(true);
            acaoPreventiva.ToTable(nameof(AcaoPreventiva).ToLower());
        }

        private void ImplementacaoEvitarReincidenciaModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ImplementacaoEvitarReincidenciaNaoConformidade>();
            entity.Property(e => e.Descricao)
                .HasConversion(StringToByteArrayConverter)
                .Metadata.SetMaxLength(null);
            entity.HasIndex(e => e.IdNaoConformidade);
            entity.ToTable(nameof(ImplementacaoEvitarReincidenciaNaoConformidade).ToLower());
        }

        private void OrdemRetrabalhoNaoConformidadeModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<OrdemRetrabalhoNaoConformidade>();
            entity.Property(e => e.MovimentacaoEstoqueMensagemRetorno)
                .HasConversion(StringToByteArrayConverter)
                .Metadata.SetMaxLength(null);

            entity.HasIndex(e => e.IdNaoConformidade);
            entity.ToTable(nameof(OrdemRetrabalhoNaoConformidade).ToLower());
        }
        private void ConfiguracaoGeralModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ConfiguracaoGeral>();
            entity.ToTable(nameof(ConfiguracaoGeral).ToLower());
        }
        private void SeederManagerModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SeederManager>();
            entity.ToTable(nameof(SeederManager).ToLower());
        }

        private void PedidoVendaModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PedidoVenda>();
            entity.ToTable(nameof(PedidoVenda).ToLower());
        }
        private void OperacaoRetrabalhoNaoConformidadeModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<OperacaoRetrabalhoNaoConformidade>();
            entity.ToTable(nameof(OperacaoRetrabalhoNaoConformidade).ToLower());

            entity
                .HasOne(operacao => operacao.NaoConformidade)
                .WithOne(naoConformidade => naoConformidade.OperacaoRetrabalho)
                .HasForeignKey<OperacaoRetrabalhoNaoConformidade>(e => e.IdNaoConformidade)
                .IsRequired();

            entity
                .HasMany(e => e.Operacoes)
                .WithOne(e => e.OperacaoRetrabalhoNaoConformidade)
                .HasForeignKey(e => e.IdOperacaoRetrabalhoNaoConformdiade)
                .IsRequired();
        }
        private void OperacaoModel(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Operacao>();
            entity.ToTable(nameof(Operacao).ToLower());

        }

        private void CentroCustoModel(ModelBuilder modelBuilder)
        {
            var centroCusto = modelBuilder.Entity<CentroCusto>();
            centroCusto.ToTable(nameof(CentroCusto).ToLower());
        }
        private void LocalModel(ModelBuilder modelBuilder)
        {
            var local = modelBuilder.Entity<Local>();
            local.ToTable(nameof(Local).ToLower());
        }
    }
}
