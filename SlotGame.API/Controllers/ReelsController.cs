using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlotGame.DataAccess.Data;
using SlotGame.Types.Models;

namespace SlotGame.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReelsController : ControllerBase
    {
        private readonly SlotGameDbContext _context;
        public ReelsController(SlotGameDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reel>>> GetReels() => await _context.Reels.Include(r => r.Symbols).ToListAsync();

        [HttpGet("GetReelsBySlotId/{id}")]
        public async Task<ActionResult<IEnumerable<Reel>>> GetReelsBySlotId(int id) => await _context.Reels.Where(r => r.SlotId == id).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Reel>> GetReel(int id)
        {
            var reel = await _context.Reels.FindAsync(id);

            if (reel == null)
                return NotFound();

            return reel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReel(int id, Reel reel)
        {
            if (id != reel.Id)
                return BadRequest();

            _context.Entry(reel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReelExists(id)) return NotFound();
                else throw;
            }

            return Ok(reel);
        }

        [HttpPost]
        public async Task<ActionResult<Reel>> AddReel(Reel reel)
        {
            _context.Reels.Add(reel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReel", new { id = reel.Id }, reel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Reel>> DeleteReel(int id)
        {
            var reel = await _context.Reels.FindAsync(id);
            if (reel == null)
                return NotFound();

            _context.Reels.Remove(reel);
            await _context.SaveChangesAsync();

            return reel;
        }

        private bool ReelExists(int id)
        {
            return _context.Reels.Any(e => e.Id == id);
        }
    }
}
