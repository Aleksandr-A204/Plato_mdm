using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plato.MDM.DataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "mdm_DirectoryDomain",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mdm_DirectoryDomain_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mdm_DirectoryLevel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mdm_DirectoryLevel_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mdm_Directory",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DirectoryDomainId = table.Column<Guid>(type: "uuid", nullable: true),
                    DirectoryLevelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mdm_Directory_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "mdm_Directory_DirectoryDomainId_fkey",
                        column: x => x.DirectoryDomainId,
                        principalSchema: "public",
                        principalTable: "mdm_DirectoryDomain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "mdm_Directory_DirectoryLevelId_fkey",
                        column: x => x.DirectoryLevelId,
                        principalSchema: "public",
                        principalTable: "mdm_DirectoryLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "mdm_DirectoryVersion",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    DirectoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataSourceName = table.Column<string>(type: "text", nullable: true),
                    DataSourceDate = table.Column<string>(type: "text", nullable: true),
                    DataSourceUrl = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VersionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    VersionDescription = table.Column<string>(type: "text", nullable: true),
                    TableName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mdm_DirectoryVersion_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "mdm_DirectoryVersion_DirectoryId_fkey",
                        column: x => x.DirectoryId,
                        principalSchema: "public",
                        principalTable: "mdm_Directory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mdm_Directory_DirectoryDomainId",
                schema: "public",
                table: "mdm_Directory",
                column: "DirectoryDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_mdm_Directory_DirectoryLevelId",
                schema: "public",
                table: "mdm_Directory",
                column: "DirectoryLevelId");

            migrationBuilder.CreateIndex(
                name: "mdm_DirectoryDomain_Name_key",
                schema: "public",
                table: "mdm_DirectoryDomain",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "mdm_DirectoryLevel_Name_key",
                schema: "public",
                table: "mdm_DirectoryLevel",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mdm_DirectoryVersion_DirectoryId",
                schema: "public",
                table: "mdm_DirectoryVersion",
                column: "DirectoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mdm_DirectoryVersion",
                schema: "public");

            migrationBuilder.DropTable(
                name: "mdm_Directory",
                schema: "public");

            migrationBuilder.DropTable(
                name: "mdm_DirectoryDomain",
                schema: "public");

            migrationBuilder.DropTable(
                name: "mdm_DirectoryLevel",
                schema: "public");
        }
    }
}
