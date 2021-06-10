using Microsoft.EntityFrameworkCore.Migrations;

namespace MainEnvironment.Database.Migrations
{
    public partial class AddInstructionsType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructionsType",
                table: "DownloadInstructions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructionsType",
                table: "DownloadInstructions");
        }
    }
}
