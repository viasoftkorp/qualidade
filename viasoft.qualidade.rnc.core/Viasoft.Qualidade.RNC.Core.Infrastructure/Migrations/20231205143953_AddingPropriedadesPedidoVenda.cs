using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingPropriedadesPedidoVenda : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingPropriedadesPedidoVenda(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "idpedido",
                table: "naoconformidade",
                type: "uuid",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "idprodutofaturamento",
                table: "naoconformidade",
                type: "uuid",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<int>(
                name: "numeroodffaturamento",
                table: "naoconformidade",
                type: "integer",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idpedido",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "idprodutofaturamento",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "numeroodffaturamento",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
