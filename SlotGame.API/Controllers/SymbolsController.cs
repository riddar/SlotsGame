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
    public class SymbolsController : ControllerBase
    {
        private readonly SlotGameDbContext _context;
        public SymbolsController(SlotGameDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Symbol>>> GetSymbols() => await _context.Symbols.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Symbol>> GetSymbol(int id)
        {
            var symbol = await _context.Symbols.FindAsync(id);

            if (symbol == null)
                return NotFound();

            return symbol;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSymbol(int id, Symbol symbol)
        {
            if (id != symbol.Id)
                return BadRequest();

            _context.Entry(symbol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SymbolExists(id)) return NotFound();
                else throw;
            }

            return Ok(symbol);
        }

        [HttpPost]
        public async Task<ActionResult<Symbol>> AddSymbol(Symbol symbol)
        {
            _context.Symbols.Add(symbol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSymbol", new { id = symbol.Id }, symbol);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Symbol>> DeleteSymbol(int id)
        {
            var symbol = await _context.Symbols.FindAsync(id);
            if (symbol == null)
                return NotFound();

            _context.Symbols.Remove(symbol);
            await _context.SaveChangesAsync();

            return symbol;
        }

        private bool SymbolExists(int id) => _context.Symbols.Any(e => e.Id == id);
    }
}
