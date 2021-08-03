using Microsoft.EntityFrameworkCore.Migrations;

namespace MainEnvironment.Database.Migrations
{
    public partial class AddGroupsSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequiredAppVersion",
                table: "Experiments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalGroups",
                table: "Experiments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "RequiredAppVersion",
                table: "Experiments");

            migrationBuilder.DropColumn(
                name: "TotalGroups",
                table: "Experiments");
        }
    }
}
