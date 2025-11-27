using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

#nullable disable

namespace Viasoft.Qualidade.RNC.Core.Infrastructure.Migrations
{
    public partial class AddingCompanyId : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingCompanyId(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "solucaonaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "serviconaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "reclamacaonaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "produtonaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "naoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "defeitonaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "conclusaonaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "causanaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<Guid>(
                name: "companyid",
                table: "acaopreventivanaoconformidade",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "companyid",
                table: "solucaonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "serviconaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "reclamacaonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "produtonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "naoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "defeitonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "conclusaonaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "causanaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "companyid",
                table: "acaopreventivanaoconformidade",
                schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
