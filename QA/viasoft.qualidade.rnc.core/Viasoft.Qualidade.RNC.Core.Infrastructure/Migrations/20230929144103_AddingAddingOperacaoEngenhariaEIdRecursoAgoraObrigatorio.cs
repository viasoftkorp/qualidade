using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingAddingOperacaoEngenhariaEIdRecursoAgoraObrigatorio : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingAddingOperacaoEngenhariaEIdRecursoAgoraObrigatorio(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "idrecurso",
                table: "servicosolucao",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AlterColumn<Guid>(
                name: "idrecurso",
                table: "serviconaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<byte[]>(
                name: "operacaoengenharia",
                table: "produtosolucao",
                type: "bytea",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<byte[]>(
                name: "operacaoengenharia",
                table: "produtonaoconformidade",
                type: "bytea",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operacaoengenharia",
                table: "produtosolucao",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "operacaoengenharia",
                table: "produtonaoconformidade",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AlterColumn<Guid>(
                name: "idrecurso",
                table: "servicosolucao",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AlterColumn<Guid>(
                name: "idrecurso",
                table: "serviconaoconformidade",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
