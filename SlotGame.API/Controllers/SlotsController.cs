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
    public class SlotsController : ControllerBase
    {
        private readonly SlotGameDbContext _context;
        public SlotsController(SlotGameDbContext context) => _context = context;
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Slot>>> GetSlots() => await _context.Slots.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Slot>> GetSlot(int id)
        {
            var slot = await _context.Slots.FindAsync(id);

            if (slot == null)
                return NotFound();

            return slot;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, Slot slot)
        {
            if (id != slot.Id)
                return BadRequest();

            _context.Entry(slot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlotExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Slot>> AddSlot(Slot slot)
        {
            _context.Slots.Add(slot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSlot", new { id = slot.Id }, slot);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Slot>> DeleteSlot(int id)
        {
            var slot = await _context.Slots.FindAsync(id);
            if (slot == null)
                return NotFound();

            _context.Slots.Remove(slot);
            await _context.SaveChangesAsync();

            return slot;
        }

        private bool SlotExists(int id) => _context.Slots.Any(e => e.Id == id);
    }
}
