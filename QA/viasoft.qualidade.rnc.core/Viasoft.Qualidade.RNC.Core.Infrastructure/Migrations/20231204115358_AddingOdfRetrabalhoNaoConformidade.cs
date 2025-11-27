using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingOdfRetrabalhoNaoConformidade : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingOdfRetrabalhoNaoConformidade(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "idproduto",
                table: "servicosolucao",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AlterColumn<Guid>(
                name: "idproduto",
                table: "serviconaoconformidade",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "idcategoria",
                table: "produto",
                type: "uuid",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<string>(
                name: "numeropedido",
                table: "naoconformidade",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "configuracaogeral",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    considerarapenassaldoapontado = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_configuracaogeral", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "ordemretrabalhonaoconformidade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    idnaoconformidade = table.Column<Guid>(type: "uuid", nullable: false),
                    idodfretrabalho = table.Column<Guid>(type: "uuid", nullable: false),
                    quantidade = table.Column<int>(type: "integer", nullable: false),
                    idlocalorigem = table.Column<Guid>(type: "uuid", nullable: false),
                    idestoquelocaldestino = table.Column<Guid>(type: "uuid", nullable: true),
                    idlocaldestino = table.Column<Guid>(type: "uuid", nullable: false),
                    movimentacaoestoquemensagemretorno = table.Column<byte[]>(type: "bytea", nullable: true),
                    codigoarmazem = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    datafabricacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    datavalidade = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("pk_ordemretrabalhonaoconformidade", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "seedermanager",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    environmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    preencheridcategoriaprodutosseederfinalizado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seedermanager", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_ordemretrabalhonaoconformidade_idnaoconformidade",
                table: "ordemretrabalhonaoconformidade",
                column: "idnaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuracaogeral",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "ordemretrabalhonaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "seedermanager",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "idcategoria",
                table: "produto",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "numeropedido",
                table: "naoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AlterColumn<Guid>(
                name: "idproduto",
                table: "servicosolucao",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AlterColumn<Guid>(
                name: "idproduto",
                table: "serviconaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
