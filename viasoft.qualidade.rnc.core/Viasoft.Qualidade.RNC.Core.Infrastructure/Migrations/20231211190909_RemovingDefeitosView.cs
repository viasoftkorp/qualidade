using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class RemovingDefeitosView : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public RemovingDefeitosView(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "defeitoview",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "defeitoview",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    causa = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    codigo = table.Column<int>(type: "integer", nullable: false),
                    codigocausa = table.Column<int>(type: "integer", nullable: true),
                    codigosolucao = table.Column<int>(type: "integer", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    deleterid = table.Column<Guid>(type: "uuid", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    descricao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    descricaocausa = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    descricaosolucao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    detalhamento = table.Column<byte[]>(type: "bytea", nullable: true),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    idcausa = table.Column<Guid>(type: "uuid", nullable: true),
                    iddefeito = table.Column<Guid>(type: "uuid", nullable: false),
                    idsolucao = table.Column<Guid>(type: "uuid", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lastmodifierid = table.Column<Guid>(type: "uuid", nullable: true),
                    solucao = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_defeitoview", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
