using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingImplementacaoEvitarReincidenciaNaoConformidade : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingImplementacaoEvitarReincidenciaNaoConformidade(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "implementacaoevitarreincidencianaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    iddefeitonaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<byte[]>(type: "bytea", nullable: true),
                    idresponsavel = table.Column<Guid>(type: "uuid", nullable: true),
                    dataanalise = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dataprevistaimplantacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    idauditor = table.Column<Guid>(type: "uuid", nullable: true),
                    dataverificacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    novadata = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    acaoimplementada = table.Column<bool>(type: "boolean", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    companyid = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_implementacaoevitarreincidencianaoconformidade", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_implementacaoevitarreincidencianaoconformidade_idnaoconform~",
                table: "implementacaoevitarreincidencianaoconformidade",
                column: "idnaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "implementacaoevitarreincidencianaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
