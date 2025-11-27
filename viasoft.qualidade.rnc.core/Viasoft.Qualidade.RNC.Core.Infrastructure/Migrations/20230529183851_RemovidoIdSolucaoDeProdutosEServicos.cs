using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class RemovidoIdSolucaoDeProdutosEServicos : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public RemovidoIdSolucaoDeProdutosEServicos(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtosolucaonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "servicosolucaonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "produtonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_produtonaoconformidade", x => x.id);
                },
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "serviconaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_serviconaoconformidade", x => x.id);
                },
                schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "serviconaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "produtosolucaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucaonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produtosolucaonaoconformidade", x => x.id);
                },
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "servicosolucaonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    horas = table.Column<int>(type: "integer", nullable: true),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idrecurso = table.Column<Guid>(type: "uuid", nullable: true),
                    idsolucaonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    minutos = table.Column<int>(type: "integer", nullable: true),
                    operacaoengenharia = table.Column<byte[]>(type: "bytea", nullable: true),
                    quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicosolucaonaoconformidade", x => x.id);
                },
                schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
