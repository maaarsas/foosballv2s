using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace foosballv2s.WebService.Migrations
{
    public partial class tournamentsv2renamedfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPairs",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTeams",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTeams",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPairs",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
