using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AdicionaSeederCentrosCustosCausasNaoConformidades : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AdicionaSeederCentrosCustosCausasNaoConformidades(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "preencheridscausascentroscustosnaoconformidadesseederfinalizado",
                table: "seedermanager",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preencheridscausascentroscustosnaoconformidadesseederfinalizado",
                table: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
