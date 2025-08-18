using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerStore.Infrastructer.Migrations
{
    public partial class addOnlineFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Passengers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Online",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "Online",
                table: "Drivers");
        }
    }
}
