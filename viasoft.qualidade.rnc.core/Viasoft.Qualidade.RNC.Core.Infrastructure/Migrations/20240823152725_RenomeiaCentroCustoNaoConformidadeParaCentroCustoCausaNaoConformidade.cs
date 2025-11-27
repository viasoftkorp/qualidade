using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class RenomeiaCentroCustoNaoConformidadeParaCentroCustoCausaNaoConformidade : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public RenomeiaCentroCustoNaoConformidadeParaCentroCustoCausaNaoConformidade(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "centrocustonaoconformidades",
                newName: "centrocustocausanaoconformidades",
                schema: _schemaNameProvider.GetSchemaName(),
                newSchema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "idcausanaoconformidade",
                table: "centrocustocausanaoconformidades",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
