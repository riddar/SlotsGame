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
    public class PayLinesController : ControllerBase
    {
        private readonly SlotGameDbContext _context;
        public PayLinesController(SlotGameDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayLine>>> GetPayLines() => await _context.PayLines.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<PayLine>> GetPayLine(int id)
        {
            var payLine = await _context.PayLines.FindAsync(id);

            if (payLine == null)
                return NotFound();

            return payLine;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayLine(int id, PayLine payLine)
        {
            if (id != payLine.Id)
                return BadRequest();

            _context.Entry(payLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayLineExists(id)) return NotFound();
                else throw;
            }

            return Ok(payLine);
        }

        [HttpPost]
        public async Task<ActionResult<PayLine>> PostPayLine(PayLine payLine)
        {
            _context.PayLines.Add(payLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayLine", new { id = payLine.Id }, payLine);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PayLine>> DeletePayLine(int id)
        {
            var payLine = await _context.PayLines.FindAsync(id);
            if (payLine == null)
                return NotFound();

            _context.PayLines.Remove(payLine);
            await _context.SaveChangesAsync();

            return payLine;
        }

        private bool PayLineExists(int id) => _context.PayLines.Any(e => e.Id == id);
    }
}
