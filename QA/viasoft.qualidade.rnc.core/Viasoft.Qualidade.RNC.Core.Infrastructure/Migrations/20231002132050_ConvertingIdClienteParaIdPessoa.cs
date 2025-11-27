using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class ConvertingIdClienteParaIdPessoa : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public ConvertingIdClienteParaIdPessoa(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idcliente",
                table: "naoconformidade",
                newName: "idpessoa",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idpessoa",
                table: "naoconformidade",
                newName: "idcliente",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
