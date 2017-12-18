using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace foosballv2s.WebService.Migrations
{
    public partial class tournamentsv52addcascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TournamentTeams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TournamentTeams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
