using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingCodigoRecurso : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingCodigoRecurso(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "preenchercodigorecursosseederfinalizado",
                table: "seedermanager",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "codigo",
                table: "recurso",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preenchercodigorecursosseederfinalizado",
                table: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "codigo",
                table: "recurso",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
