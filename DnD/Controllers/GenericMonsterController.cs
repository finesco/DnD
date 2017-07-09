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