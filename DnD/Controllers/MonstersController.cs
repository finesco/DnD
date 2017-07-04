using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DnD.DTO;
using DnD.Data;

namespace DnD.Controllers
{
    [Route("api/Monsters")]
    public class MonstersController : Controller
    {
        private readonly DnDContext _context;

        public MonstersController(DnDContext context)
        {
            _context = context;
        }

        [HttpPost]
        public void PostMonsters([FromBody] IEnumerable<Monster> monsters)
        {
            foreach (var monster in monsters)
                _context.Add(monster);
            _context.SaveChanges();
        }
    }
}