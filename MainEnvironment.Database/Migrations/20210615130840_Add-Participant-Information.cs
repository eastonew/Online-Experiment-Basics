using Microsoft.EntityFrameworkCore.Migrations;

namespace MainEnvironment.Database.Migrations
{
    public partial class AddParticipantInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantInformationSheet",
                table: "Experiments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantInformationSheet",
                table: "Experiments");
        }
    }
}
