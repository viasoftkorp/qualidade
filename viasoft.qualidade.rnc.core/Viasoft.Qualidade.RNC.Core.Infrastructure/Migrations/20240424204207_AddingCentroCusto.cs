using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingCentroCusto : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingCentroCusto(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "preenchercentrocustosseederfinalizado",
                table: "seedermanager",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "centrocusto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    codigo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    issintetico = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_centrocusto", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "centrocusto",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "preenchercentrocustosseederfinalizado",
                table: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
