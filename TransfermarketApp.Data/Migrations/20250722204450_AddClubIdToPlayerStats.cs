using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransfermarketApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClubIdToPlayerStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "PlayerStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_ClubId",
                table: "PlayerStats",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Clubs_ClubId",
                table: "PlayerStats",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Clubs_ClubId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_ClubId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "PlayerStats");
        }
    }
}
