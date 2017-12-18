using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace foosballv2s.WebService.Migrations
{
    public partial class tournamentsv4rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGames_Games_GameId",
                table: "TournamentGames");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGames_Teams_Team1Id",
                table: "TournamentGames");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGames_Teams_Team2Id",
                table: "TournamentGames");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGames_Tournaments_TournamentId",
                table: "TournamentGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentGames",
                table: "TournamentGames");

            migrationBuilder.RenameTable(
                name: "TournamentGames",
                newName: "TournamentPairs");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentGames_TournamentId",
                table: "TournamentPairs",
                newName: "IX_TournamentPairs_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentGames_Team2Id",
                table: "TournamentPairs",
                newName: "IX_TournamentPairs_Team2Id");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentGames_Team1Id",
                table: "TournamentPairs",
                newName: "IX_TournamentPairs_Team1Id");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentGames_Id",
                table: "TournamentPairs",
                newName: "IX_TournamentPairs_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentGames_GameId",
                table: "TournamentPairs",
                newName: "IX_TournamentPairs_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPairs",
                table: "TournamentPairs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPairs_Games_GameId",
                table: "TournamentPairs",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPairs_Teams_Team1Id",
                table: "TournamentPairs",
                column: "Team1Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPairs_Teams_Team2Id",
                table: "TournamentPairs",
                column: "Team2Id",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPairs_Tournaments_TournamentId",
                table: "TournamentPairs",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPairs_Games_GameId",
                table: "TournamentPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPairs_Teams_Team1Id",
                table: "TournamentPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPairs_Teams_Team2Id",
                table: "TournamentPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPairs_Tournaments_TournamentId",
                table: "TournamentPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPairs",
                table: "TournamentPairs");

            migrationBuilder.RenameTable(
                name: "TournamentPairs",
                newName: "TournamentGames");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPairs_TournamentId",
                table: "TournamentGames",
                newName: "IX_TournamentGames_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPairs_Team2Id",
                table: "TournamentGames",
                newName: "IX_TournamentGames_Team2Id");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPairs_Team1Id",
                table: "TournamentGames",
                newName: "IX_TournamentGames_Team1Id");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPairs_Id",
                table: "TournamentGames",
                newName: "IX_TournamentGames_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPairs_GameId",
                table: "TournamentGames",
                newName: "IX_TournamentGames_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentGames",
                table: "TournamentGames",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentGames_Games_GameId",
                table: "TournamentGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentGames_Tournaments_TournamentId",
                table: "TournamentGames",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
