using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransfermarketApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CurrentClubNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Clubs_CurrentClubId",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentClubId",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Clubs_CurrentClubId",
                table: "Players",
                column: "CurrentClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Clubs_CurrentClubId",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentClubId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Clubs_CurrentClubId",
                table: "Players",
                column: "CurrentClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
