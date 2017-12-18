using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace foosballv2s.WebService.Migrations
{
    public partial class tournamentsv5renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTeams",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTeamsRequired",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTeamsRequired",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTeams",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
