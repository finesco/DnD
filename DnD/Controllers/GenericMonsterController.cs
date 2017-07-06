using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DnD.DTO;
using DnD.Data;
using DnD.Models;

namespace DnD.Controllers
{
    [Produces("application/json")]
    [Route("api/GenericMonster")]
    public class GenericMonsterController : Controller
    {
        private readonly DnDContext _context;

        public GenericMonsterController(DnDContext context)
        {
            _context = context;
        }

        private async Task<Character> getGenericMonster(string cr)
        {
            var genericMonsterStats = await _context.GenericMonsterStats.SingleOrDefaultAsync(m => m.CR == cr);
            if (genericMonsterStats == null)
                return null;
            Character c = new Character($"Generic CR {cr}", false, Locations.Front, genericMonsterStats.ArmorClass, genericMonsterStats.HitPoints, 1,
                2 + genericMonsterStats.ProficiencyBonus, 1, 2 + genericMonsterStats.ProficiencyBonus, 1, 1, 1);
            c.Actions.Add(new CombatAction("Generic Melee", genericMonsterStats.AttackBonus, "d10", (genericMonsterStats.Damage - 11) / 2, 2, AttackTypes.Melee));
            return c;
        }

        // GET: api/GenericMonster/5
        [HttpGet("{cr}")]
        public async Task<IActionResult> GetGenericMonsterStats([FromRoute] string cr)
        {
            var genericMonsterStats = await _context.GenericMonsterStats.SingleOrDefaultAsync(m => m.CR == cr);

            if (genericMonsterStats == null)
            {
                return NotFound();
            }

            return Ok(genericMonsterStats);
        }

        private bool GenericMonsterStatsExists(string cr)
        {
            return _context.GenericMonsterStats.Any(e => e.CR == cr);
        }
    }
}