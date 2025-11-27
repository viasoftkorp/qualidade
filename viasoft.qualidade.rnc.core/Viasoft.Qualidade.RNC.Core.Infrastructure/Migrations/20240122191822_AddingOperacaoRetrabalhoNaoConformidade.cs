using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingOperacaoRetrabalhoNaoConformidade : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingOperacaoRetrabalhoNaoConformidade(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operacaoretrabalhonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    numerooperacaoaretrabalhar = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
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
                    table.PrimaryKey("pk_operacaoretrabalhonaoconformidade", x => x.id);
                    table.ForeignKey(
                        name: "fk_operacaoretrabalhonaoconformidade_naoconformidade_idnaoconf~",
                        column: x => x.idnaoconformidade,
                        principalTable: "naoconformidade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade,
                        principalSchema: _schemaNameProvider.GetSchemaName());
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "operacao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    numerooperacao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    idrecurso = table.Column<Guid>(type: "uuid", nullable: false),
                    idoperacaoretrabalhonaoconformdiade = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_operacao", x => x.id);
                    table.ForeignKey(
                        name: "fk_operacao_operacaoretrabalhonaoconformidade_idoperacaoretrab~",
                        column: x => x.idoperacaoretrabalhonaoconformdiade,
                        principalTable: "operacaoretrabalhonaoconformidade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade,
                        principalSchema: _schemaNameProvider.GetSchemaName());
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_operacao_idoperacaoretrabalhonaoconformdiade",
                table: "operacao",
                column: "idoperacaoretrabalhonaoconformdiade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_operacaoretrabalhonaoconformidade_idnaoconformidade",
                table: "operacaoretrabalhonaoconformidade",
                column: "idnaoconformidade",
                unique: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operacao",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "operacaoretrabalhonaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
