using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class ConvertingRevisaoParaString : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public ConvertingRevisaoParaString(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "revisao",
                table: "naoconformidade",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "revisao",
                table: "naoconformidade",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450,
                oldNullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
