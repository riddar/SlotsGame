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
    public class PayoutsController : ControllerBase
    {
        private readonly SlotGameDbContext _context;
        public PayoutsController(SlotGameDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payout>>> GetPayouts() => await _context.Payouts.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Payout>> GetPayout(int id)
        {
            var payout = await _context.Payouts.FindAsync(id);

            if (payout == null)
                return NotFound();

            return payout;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayout(int id, Payout payout)
        {
            if (id != payout.Id)
                return BadRequest();

            _context.Entry(payout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayoutExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Payout>> AddPayout(Payout payout)
        {
            _context.Payouts.Add(payout);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayout", new { id = payout.Id }, payout);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Payout>> DeletePayout(int id)
        {
            var payout = await _context.Payouts.FindAsync(id);
            if (payout == null)
                return NotFound();

            _context.Payouts.Remove(payout);
            await _context.SaveChangesAsync();

            return payout;
        }

        private bool PayoutExists(int id) => _context.Payouts.Any(e => e.Id == id);
    }
}
