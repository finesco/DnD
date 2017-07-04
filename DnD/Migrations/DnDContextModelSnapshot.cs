using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DnD.Data;

namespace DnD.Migrations
{
    [DbContext(typeof(DnDContext))]
    partial class DnDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DnD.DTO.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttackBonus");

                    b.Property<int>("DamageBonus");

                    b.Property<string>("DamageDice");

                    b.Property<string>("Desc");

                    b.Property<int>("MonsterId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("MonsterId");

                    b.ToTable("MonsterActions");
                });

            modelBuilder.Entity("DnD.DTO.LegendaryAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttackBonus");

                    b.Property<int>("DamageBonus");

                    b.Property<string>("DamageDice");

                    b.Property<string>("Desc");

                    b.Property<int>("MonsterId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("MonsterId");

                    b.ToTable("MonsterLegendaryActions");
                });

            modelBuilder.Entity("DnD.DTO.Monster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Acrobatics");

                    b.Property<string>("Alignment");

                    b.Property<int>("AnimalHandling");

                    b.Property<int>("Arcana");

                    b.Property<int>("ArmorClass");

                    b.Property<int>("Athletics");

                    b.Property<string>("ChallengeRating");

                    b.Property<int>("Charisma");

                    b.Property<int>("CharismaSave");

                    b.Property<string>("ConditionImmunities");

                    b.Property<int>("Constitution");

                    b.Property<int>("ConstitutionSave");

                    b.Property<string>("DamageImmunities");

                    b.Property<string>("DamageResistances");

                    b.Property<string>("DamageVulnerabilities");

                    b.Property<int>("Deception");

                    b.Property<int>("Dexterity");

                    b.Property<int>("DexteritySave");

                    b.Property<int>("History");

                    b.Property<string>("HitDice");

                    b.Property<int>("HitPoints");

                    b.Property<int>("Insight");

                    b.Property<int>("Intelligence");

                    b.Property<int>("IntelligenceSave");

                    b.Property<int>("Intimidation");

                    b.Property<int>("Investigation");

                    b.Property<string>("Languages");

                    b.Property<int>("Medicine");

                    b.Property<string>("Name");

                    b.Property<int>("Nature");

                    b.Property<int>("Perception");

                    b.Property<int>("Performance");

                    b.Property<int>("Persuasion");

                    b.Property<int>("Religion");

                    b.Property<string>("Senses");

                    b.Property<string>("Size");

                    b.Property<int>("SleightOfHand");

                    b.Property<string>("Speed");

                    b.Property<int>("Stealth");

                    b.Property<int>("Strength");

                    b.Property<int>("StrengthSave");

                    b.Property<string>("SubType");

                    b.Property<int>("Survival");

                    b.Property<string>("Type");

                    b.Property<int>("Wisdom");

                    b.Property<int>("WisdomSave");

                    b.HasKey("Id");

                    b.ToTable("Monsters");
                });

            modelBuilder.Entity("DnD.DTO.SpecialAbility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttackBonus");

                    b.Property<string>("Desc");

                    b.Property<int>("MonsterId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("MonsterId");

                    b.ToTable("MonsterSpecialAbilities");
                });

            modelBuilder.Entity("DnD.DTO.Action", b =>
                {
                    b.HasOne("DnD.DTO.Monster", "Monster")
                        .WithMany("Actions")
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DnD.DTO.LegendaryAction", b =>
                {
                    b.HasOne("DnD.DTO.Monster", "Monster")
                        .WithMany("LegendaryActions")
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DnD.DTO.SpecialAbility", b =>
                {
                    b.HasOne("DnD.DTO.Monster", "Monster")
                        .WithMany("SpecialAbilities")
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
