using Microsoft.EntityFrameworkCore.Migrations;

namespace Curry.DataAccess.Migrations
{
    public partial class ordersupdateextra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extra",
                table: "OrderHeaders",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extra",
                table: "OrderHeaders");
        }
    }
}
