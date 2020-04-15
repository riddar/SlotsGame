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
    public class WalletsController : ControllerBase
    {
        private readonly SlotGameDbContext _context;
        public WalletsController(SlotGameDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets() => await _context.Wallets.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);

            if (wallet == null)
                return NotFound();

            return wallet;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWallet(int id, Wallet wallet)
        {
            if (id != wallet.Id)
                return BadRequest();

            _context.Entry(wallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Wallet>> AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWallet", new { id = wallet.Id }, wallet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Wallet>> DeleteWallet(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
                return NotFound();

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();

            return wallet;
        }

        private bool WalletExists(int id) => _context.Wallets.Any(e => e.Id == id);
    }
}
