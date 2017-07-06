using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DnD.Migrations
{
    public partial class addgenericmonsterstats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenericMonsterStats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArmorClass = table.Column<int>(nullable: false),
                    AttackBonus = table.Column<int>(nullable: false),
                    CR = table.Column<string>(nullable: true),
                    Damage = table.Column<int>(nullable: false),
                    HitPoints = table.Column<int>(nullable: false),
                    ProficiencyBonus = table.Column<int>(nullable: false),
                    SaveDC = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericMonsterStats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenericMonsterStats");
        }
    }
}
