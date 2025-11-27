using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingStatusOrdemRetrabalho : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingStatusOrdemRetrabalho(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "ordemretrabalhonaoconformidade",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "ordemretrabalhonaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
