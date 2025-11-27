using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingIsAtivo : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingIsAtivo(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isativo",
                table: "solucao",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<bool>(
                name: "isativo",
                table: "natureza",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<bool>(
                name: "isativo",
                table: "defeito",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<bool>(
                name: "isativo",
                table: "causa",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<bool>(
                name: "isativo",
                table: "acaopreventiva",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isativo",
                table: "solucao",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "isativo",
                table: "natureza",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "isativo",
                table: "defeito",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "isativo",
                table: "causa",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "isativo",
                table: "acaopreventiva",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
