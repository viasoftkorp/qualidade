using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class UpdatedQuantidadeTipoOrdemRetrabalho : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public UpdatedQuantidadeTipoOrdemRetrabalho(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "quantidade",
                table: "ordemretrabalhonaoconformidade",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "quantidade",
                table: "ordemretrabalhonaoconformidade",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
