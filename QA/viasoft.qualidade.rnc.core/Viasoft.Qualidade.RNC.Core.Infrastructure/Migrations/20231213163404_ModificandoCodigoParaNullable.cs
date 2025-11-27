using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class ModificandoCodigoParaNullable : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public ModificandoCodigoParaNullable(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "codigo",
                table: "naoconformidade",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "codigo",
                table: "naoconformidade",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
