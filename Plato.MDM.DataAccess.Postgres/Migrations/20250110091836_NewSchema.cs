using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plato.MDM.DataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class NewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mdm");

            migrationBuilder.RenameTable(
                name: "mdm_DirectoryVersion",
                schema: "public",
                newName: "mdm_DirectoryVersion",
                newSchema: "mdm");

            migrationBuilder.RenameTable(
                name: "mdm_DirectoryLevel",
                schema: "public",
                newName: "mdm_DirectoryLevel",
                newSchema: "mdm");

            migrationBuilder.RenameTable(
                name: "mdm_DirectoryDomain",
                schema: "public",
                newName: "mdm_DirectoryDomain",
                newSchema: "mdm");

            migrationBuilder.RenameTable(
                name: "mdm_Directory",
                schema: "public",
                newName: "mdm_Directory",
                newSchema: "mdm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "mdm_DirectoryVersion",
                schema: "mdm",
                newName: "mdm_DirectoryVersion",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "mdm_DirectoryLevel",
                schema: "mdm",
                newName: "mdm_DirectoryLevel",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "mdm_DirectoryDomain",
                schema: "mdm",
                newName: "mdm_DirectoryDomain",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "mdm_Directory",
                schema: "mdm",
                newName: "mdm_Directory",
                newSchema: "public");
        }
    }
}
