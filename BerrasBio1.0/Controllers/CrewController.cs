using BerrasBio1._0.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio1._0.Controllers
{
    public class CrewController : Controller
    {
        private readonly CinemaContext _db;
        public CrewController(CinemaContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Actors()
        {
            return View(await _db.Actors.ToListAsync());
        }
        public async Task<IActionResult> Producers()
        {
            return View(await _db.Producers.ToListAsync());
        }
        public async Task<IActionResult> Directors()
        {
            return View(await _db.Directors.ToListAsync());
        }
        
    }
}
