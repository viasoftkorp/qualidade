using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AdicionaProdutoEmpresa : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AdicionaProdutoEmpresa(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "produtoempresa",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    idproduto = table.Column<Guid>(type: "uuid", nullable: false),
                    idempresa = table.Column<Guid>(type: "uuid", nullable: false),
                    idcategoria = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_produtoempresa", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "seedermanagerporempresa",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    companyid = table.Column<Guid>(type: "uuid", nullable: false),
                    inserirprodutosempresasseederfinalizado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seedermanagerporempresa", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_produtoempresa_idproduto_idempresa",
                table: "produtoempresa",
                columns: new[] { "idproduto", "idempresa" },
                unique: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtoempresa",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "seedermanagerporempresa",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
