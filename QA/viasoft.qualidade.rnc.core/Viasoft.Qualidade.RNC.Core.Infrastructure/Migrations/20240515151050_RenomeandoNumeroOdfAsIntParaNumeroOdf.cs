using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class RenomeandoNumeroOdfAsIntParaNumeroOdf : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public RenomeandoNumeroOdfAsIntParaNumeroOdf(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numeroodfasint",
                table: "naoconformidade",
                newName: "numeroodf",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numeroodf",
                table: "naoconformidade",
                newName: "numeroodfasint",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
