using Microsoft.EntityFrameworkCore.Migrations;

namespace gymit.Migrations
{
    public partial class UpdatedTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_testItems",
                table: "testItems");

            migrationBuilder.RenameTable(
                name: "testItems",
                newName: "Tests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tests",
                table: "Tests",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tests",
                table: "Tests");

            migrationBuilder.RenameTable(
                name: "Tests",
                newName: "testItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_testItems",
                table: "testItems",
                column: "ID");
        }
    }
}
