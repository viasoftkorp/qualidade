using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class Adding_Entities : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public Adding_Entities(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "acaopreventiva",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '1', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    idresponsavel = table.Column<Guid>(type: "uuid", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_acaopreventiva", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "acaopreventivanaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idacaopreventiva = table.Column<Guid>(type: "uuid", nullable: false),
                    iddefeitonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    acao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    idresponsavel = table.Column<Guid>(type: "uuid", nullable: true),
                    dataanalise = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    dataprevistaimplantacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    idauditor = table.Column<Guid>(type: "uuid", nullable: false),
                    implementada = table.Column<bool>(type: "boolean", nullable: false),
                    dataverificacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    novadata = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_acaopreventivanaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "causa",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '1', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_causa", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "causanaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    iddefeitonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idcausa = table.Column<Guid>(type: "uuid", nullable: false),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_causanaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    razaosocial = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cliente", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "conclusaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    novareuniao = table.Column<bool>(type: "boolean", nullable: false),
                    datareuniao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    dataverificacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    idauditor = table.Column<Guid>(type: "uuid", nullable: false),
                    evidencia = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    eficaz = table.Column<bool>(type: "boolean", nullable: false),
                    ciclodetempo = table.Column<int>(type: "integer", nullable: false),
                    idnovorelatorio = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conclusaonaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "defeito",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '1', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    idcausa = table.Column<Guid>(type: "uuid", nullable: true),
                    idsolucao = table.Column<Guid>(type: "uuid", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_defeito", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "defeitonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    iddefeito = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_defeitonaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "defeitoview",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    iddefeito = table.Column<Guid>(type: "uuid", nullable: false),
                    idcausa = table.Column<Guid>(type: "uuid", nullable: true),
                    idsolucao = table.Column<Guid>(type: "uuid", nullable: true),
                    descricaocausa = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    codigocausa = table.Column<int>(type: "integer", nullable: true),
                    descricaosolucao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    codigosolucao = table.Column<int>(type: "integer", nullable: true),
                    causa = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    solucao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_defeitoview", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "naoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<int>(type: "integer", nullable: false),
                    origem = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    idnotafiscal = table.Column<Guid>(type: "uuid", nullable: true),
                    numeronotafiscal = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    idnatureza = table.Column<Guid>(type: "uuid", nullable: false),
                    idcliente = table.Column<Guid>(type: "uuid", nullable: true),
                    idodf = table.Column<Guid>(type: "uuid", nullable: true),
                    numeroodf = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idlote = table.Column<Guid>(type: "uuid", nullable: true),
                    numerolote = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    datafabricacaolote = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    camponf = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    idcriador = table.Column<Guid>(type: "uuid", nullable: false),
                    revisao = table.Column<int>(type: "integer", nullable: false),
                    lotetotal = table.Column<bool>(type: "boolean", nullable: false),
                    loteparcial = table.Column<bool>(type: "boolean", nullable: false),
                    rejeitado = table.Column<bool>(type: "boolean", nullable: false),
                    aceitoconcessao = table.Column<bool>(type: "boolean", nullable: false),
                    retrabalhopelocliente = table.Column<bool>(type: "boolean", nullable: false),
                    retrabalhonocliente = table.Column<bool>(type: "boolean", nullable: false),
                    equipe = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    naoconformidadeempotencial = table.Column<bool>(type: "boolean", nullable: false),
                    relatonaoconformidade = table.Column<bool>(type: "boolean", nullable: false),
                    melhoriaempotencial = table.Column<bool>(type: "boolean", nullable: false),
                    descricao = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_naoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "natureza",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '1', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_natureza", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    idunidademedida = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produto", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "produtosolucao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucao = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade = table.Column<int>(type: "integer", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produtosolucao", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "produtosolucaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucaonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produtosolucaonaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "reclamacaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    procedentes = table.Column<int>(type: "integer", nullable: false),
                    improcedentes = table.Column<int>(type: "integer", nullable: false),
                    quantidadelote = table.Column<decimal>(type: "numeric", nullable: false),
                    quantidadenaoconformidade = table.Column<decimal>(type: "numeric", nullable: false),
                    disposicaoprodutosaprovados = table.Column<int>(type: "integer", nullable: false),
                    disposicaoprodutosconcessao = table.Column<int>(type: "integer", nullable: false),
                    retrabalho = table.Column<int>(type: "integer", nullable: false),
                    rejeitado = table.Column<int>(type: "integer", nullable: false),
                    retrabalhocomonus = table.Column<bool>(type: "boolean", nullable: false),
                    retrabalhosemonus = table.Column<bool>(type: "boolean", nullable: false),
                    devolucaofornecedor = table.Column<bool>(type: "boolean", nullable: false),
                    recodificar = table.Column<bool>(type: "boolean", nullable: false),
                    sucata = table.Column<bool>(type: "boolean", nullable: false),
                    observacao = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reclamacaonaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "recurso",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recurso", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "servicosolucao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucao = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade = table.Column<int>(type: "integer", nullable: false),
                    horas = table.Column<int>(type: "integer", nullable: true),
                    minutos = table.Column<int>(type: "integer", nullable: true),
                    idrecurso = table.Column<Guid>(type: "uuid", nullable: true),
                    operacaoengenharia = table.Column<byte[]>(type: "bytea", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicosolucao", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "servicosolucaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucaonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    horas = table.Column<int>(type: "integer", nullable: true),
                    minutos = table.Column<int>(type: "integer", nullable: true),
                    idrecurso = table.Column<Guid>(type: "uuid", nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    operacaoengenharia = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicosolucaonaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "solucao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    imediata = table.Column<bool>(type: "boolean", nullable: false),
                    codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '1', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_solucao", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "solucaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    iddefeitonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucao = table.Column<Guid>(type: "uuid", nullable: false),
                    solucaoimediata = table.Column<bool>(type: "boolean", nullable: false),
                    dataanalise = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dataprevistaimplantacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    idresponsavel = table.Column<Guid>(type: "uuid", nullable: true),
                    custoestimado = table.Column<decimal>(type: "numeric", nullable: false),
                    novadata = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dataverificacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    idauditor = table.Column<Guid>(type: "uuid", nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_solucaonaoconformidade", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "unidademedidaproduto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    codigo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unidademedidaproduto", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    sobrenome = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario", x => x.id);
                },schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_naoconformidade_codigo",
                table: "naoconformidade",
                column: "codigo",
                unique: true,
                filter: "\"isdeleted\" = false",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "acaopreventiva");

            migrationBuilder.DropTable(
                name: "acaopreventivanaoconformidade");

            migrationBuilder.DropTable(
                name: "causa");

            migrationBuilder.DropTable(
                name: "causanaoconformidade");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "conclusaonaoconformidade");

            migrationBuilder.DropTable(
                name: "defeito");

            migrationBuilder.DropTable(
                name: "defeitonaoconformidade");

            migrationBuilder.DropTable(
                name: "defeitoview");

            migrationBuilder.DropTable(
                name: "naoconformidade");

            migrationBuilder.DropTable(
                name: "natureza");

            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "produtosolucao");

            migrationBuilder.DropTable(
                name: "produtosolucaonaoconformidade");

            migrationBuilder.DropTable(
                name: "reclamacaonaoconformidade");

            migrationBuilder.DropTable(
                name: "recurso");

            migrationBuilder.DropTable(
                name: "servicosolucao");

            migrationBuilder.DropTable(
                name: "servicosolucaonaoconformidade");

            migrationBuilder.DropTable(
                name: "solucao");

            migrationBuilder.DropTable(
                name: "solucaonaoconformidade");

            migrationBuilder.DropTable(
                name: "unidademedidaproduto");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
