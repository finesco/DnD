using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnD.DTO;

namespace DnD.Data
{
    public class DnDContext : DbContext
    {
        public DnDContext(DbContextOptions<DnDContext> options)
            : base(options)
        { }

        public DbSet<Monster> Monsters { get; set; }
        public DbSet<SpecialAbility> MonsterSpecialAbilities { get; set; }
        public DbSet<DTO.Action> MonsterActions { get; set; }
        public DbSet<LegendaryAction> MonsterLegendaryActions { get; set; }
        public DbSet<GenericMonsterStats> GenericMonsterStats { get; set; }
    }
}
