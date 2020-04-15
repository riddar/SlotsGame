using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlotGame.Types.Models;

namespace SlotGame.DataAccess.Data
{
    public class SlotGameDbContext : IdentityDbContext
    {
        public SlotGameDbContext(DbContextOptions<SlotGameDbContext> options) : base(options) { }

        public DbSet<PayLine> PayLines { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        public DbSet<Reel> Reels { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //Add Entities
            builder.Entity<PayLine>().HasKey(p => p.Id);
            builder.Entity<Payout>().HasKey(p => p.Id);
            builder.Entity<Reel>().HasKey(r => r.Id);
            builder.Entity<Slot>().HasKey(s => s.Id);
            builder.Entity<Symbol>().HasKey(s => s.Id);
            builder.Entity<Wallet>().HasKey(w => w.Id);
            builder.Entity<Wallet>().HasMany(w => w.Slots).WithOne(s => s.Wallet);
        }
    }
}
