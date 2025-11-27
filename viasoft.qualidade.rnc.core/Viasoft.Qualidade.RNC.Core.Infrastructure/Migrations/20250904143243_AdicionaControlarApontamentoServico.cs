using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AdicionaControlarApontamentoServico : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AdicionaControlarApontamentoServico(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "controlarapontamento",
                table: "serviconaoconformidade",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "controlarapontamento",
                table: "serviconaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
