using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingNumeroOdfAsInt : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingNumeroOdfAsInt(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "converternumeroodfparanumeroodfasintseederfinalizado",
                table: "seedermanager",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<int>(
                name: "numeroodfasint",
                table: "naoconformidade",
                type: "integer",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "converternumeroodfparanumeroodfasintseederfinalizado",
                table: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "numeroodfasint",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
