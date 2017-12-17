using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace foosballv2s.WebService.Migrations
{
    public partial class tournamentsv3rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Team1Id",
                table: "TournamentGames",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Team2Id",
                table: "TournamentGames",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_Id",
                table: "Tournaments",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGames_Id",
                table: "TournamentGames",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGames_Team1Id",
                table: "TournamentGames",
                column: "Team1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGames_Team2Id",
                table: "TournamentGames",
                column: "Team2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentGames_Teams_Team1Id",
                table: "TournamentGames",
                column: "Team1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentGames_Teams_Team2Id",
                table: "TournamentGames",
                column: "Team2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGames_Teams_Team1Id",
                table: "TournamentGames");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGames_Teams_Team2Id",
                table: "TournamentGames");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_Id",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_TournamentGames_Id",
                table: "TournamentGames");

            migrationBuilder.DropIndex(
                name: "IX_TournamentGames_Team1Id",
                table: "TournamentGames");

            migrationBuilder.DropIndex(
                name: "IX_TournamentGames_Team2Id",
                table: "TournamentGames");

            migrationBuilder.DropColumn(
                name: "Team1Id",
                table: "TournamentGames");

            migrationBuilder.DropColumn(
                name: "Team2Id",
                table: "TournamentGames");
        }
    }
}
