using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class ConvertingIdOdfRetrabalhoParaNumeroOdfRetrabalho : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public ConvertingIdOdfRetrabalhoParaNumeroOdfRetrabalho(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idodfretrabalho",
                table: "ordemretrabalhonaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<int>(
                name: "numeroodfretrabalho",
                table: "ordemretrabalhonaoconformidade",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numeroodfretrabalho",
                table: "ordemretrabalhonaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "idodfretrabalho",
                table: "ordemretrabalhonaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
