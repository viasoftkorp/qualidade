using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingNumeroOdfAndRemoveIdOdf : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingNumeroOdfAndRemoveIdOdf(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idodf",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "idodf",
                table: "naoconformidade",
                type: "uuid",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
