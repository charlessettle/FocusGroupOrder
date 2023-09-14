using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Focus.Migrations
{
    public partial class creatorOfOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Orders");
        }
    }
}
