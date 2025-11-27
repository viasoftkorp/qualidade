using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingPreencherLocaisSeederFinalizado : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingPreencherLocaisSeederFinalizado(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "preencherlocaisseederfinalizado",
                table: "seedermanager",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "local",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    isbloquearmovimentacao = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_local", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "local",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "preencherlocaisseederfinalizado",
                table: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
