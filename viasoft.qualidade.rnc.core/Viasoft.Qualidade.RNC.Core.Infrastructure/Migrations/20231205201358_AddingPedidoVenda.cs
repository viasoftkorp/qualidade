using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingPedidoVenda : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingPedidoVenda(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pedidovenda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    numeropedido = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedidovenda", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pedidovenda",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
