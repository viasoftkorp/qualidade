using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class ConvertendoIdPedidoParaNumeroPedido : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public ConvertendoIdPedidoParaNumeroPedido(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "converteridpedidoparanumeropedidoseederfinalizado",
                table: "seedermanager",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "numeropedido",
                table: "naoconformidade",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "converteridpedidoparanumeropedidoseederfinalizado",
                table: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "numeropedido",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
