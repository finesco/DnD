using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DnD.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Acrobatics = table.Column<int>(nullable: false),
                    Alignment = table.Column<string>(nullable: true),
                    AnimalHandling = table.Column<int>(nullable: false),
                    Arcana = table.Column<int>(nullable: false),
                    ArmorClass = table.Column<int>(nullable: false),
                    Athletics = table.Column<int>(nullable: false),
                    ChallengeRating = table.Column<string>(nullable: true),
                    Charisma = table.Column<int>(nullable: false),
                    CharismaSave = table.Column<int>(nullable: false),
                    ConditionImmunities = table.Column<string>(nullable: true),
                    Constitution = table.Column<int>(nullable: false),
                    ConstitutionSave = table.Column<int>(nullable: false),
                    DamageImmunities = table.Column<string>(nullable: true),
                    DamageResistances = table.Column<string>(nullable: true),
                    DamageVulnerabilities = table.Column<string>(nullable: true),
                    Deception = table.Column<int>(nullable: false),
                    Dexterity = table.Column<int>(nullable: false),
                    DexteritySave = table.Column<int>(nullable: false),
                    History = table.Column<int>(nullable: false),
                    HitDice = table.Column<string>(nullable: true),
                    HitPoints = table.Column<int>(nullable: false),
                    Insight = table.Column<int>(nullable: false),
                    Intelligence = table.Column<int>(nullable: false),
                    IntelligenceSave = table.Column<int>(nullable: false),
                    Intimidation = table.Column<int>(nullable: false),
                    Investigation = table.Column<int>(nullable: false),
                    Languages = table.Column<string>(nullable: true),
                    Medicine = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Nature = table.Column<int>(nullable: false),
                    Perception = table.Column<int>(nullable: false),
                    Performance = table.Column<int>(nullable: false),
                    Persuasion = table.Column<int>(nullable: false),
                    Religion = table.Column<int>(nullable: false),
                    Senses = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    SleightOfHand = table.Column<int>(nullable: false),
                    Speed = table.Column<string>(nullable: true),
                    Stealth = table.Column<int>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    StrengthSave = table.Column<int>(nullable: false),
                    SubType = table.Column<string>(nullable: true),
                    Survival = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Wisdom = table.Column<int>(nullable: false),
                    WisdomSave = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonsterActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttackBonus = table.Column<int>(nullable: false),
                    DamageBonus = table.Column<int>(nullable: false),
                    DamageDice = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    MonsterId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonsterActions_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonsterLegendaryActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttackBonus = table.Column<int>(nullable: false),
                    DamageBonus = table.Column<int>(nullable: false),
                    DamageDice = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    MonsterId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterLegendaryActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonsterLegendaryActions_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonsterSpecialAbilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttackBonus = table.Column<int>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    MonsterId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterSpecialAbilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonsterSpecialAbilities_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonsterActions_MonsterId",
                table: "MonsterActions",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterLegendaryActions_MonsterId",
                table: "MonsterLegendaryActions",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterSpecialAbilities_MonsterId",
                table: "MonsterSpecialAbilities",
                column: "MonsterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonsterActions");

            migrationBuilder.DropTable(
                name: "MonsterLegendaryActions");

            migrationBuilder.DropTable(
                name: "MonsterSpecialAbilities");

            migrationBuilder.DropTable(
                name: "Monsters");
        }
    }
}
