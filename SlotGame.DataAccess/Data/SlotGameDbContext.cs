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
            builder.Entity<Reel>().HasMany(r => r.Symbols).WithOne(s => s.Reel);
            builder.Entity<Slot>().HasKey(s => s.Id);
            builder.Entity<Symbol>().HasKey(s => s.Id);
            builder.Entity<Symbol>().HasOne(s => s.Reel).WithMany(r => r.Symbols);
            builder.Entity<Wallet>().HasKey(w => w.Id);
            builder.Entity<Wallet>().HasMany(w => w.Slots).WithOne(s => s.Wallet);


            //seed
            builder.Entity<Wallet>().HasData(new Wallet { Id=1, Balance = 5000 });
            builder.Entity<Slot>().HasData(new Slot { Id=1, WalletId=1 });

            builder.Entity<Reel>().HasData(new Reel { Id = 1, SlotId = 1, Speed=150, SymbolOrder= "1113411134111" });
            builder.Entity<Reel>().HasData(new Reel { Id = 2, SlotId = 1, Speed = 150, SymbolOrder = "0123402340340" });           
            builder.Entity<Reel>().HasData(new Reel { Id = 3, SlotId = 1, Speed = 150, SymbolOrder = "0123434034000" });           
            builder.Entity<Reel>().HasData(new Reel { Id = 4, SlotId = 1, Speed = 150, SymbolOrder = "1111111111111" });
            builder.Entity<Reel>().HasData(new Reel { Id = 5, SlotId = 1, Speed = 150, SymbolOrder = "1111111111111" });

            builder.Entity<Symbol>().HasData(new Symbol { Id=1, ReelId=1});             
        }                                                                               
    }                                                                                   
}                                                                                       
                                                                                        
                                                                                        
                                                                                        
                                                                                        
                                                                                        
                                                                                        
                                                                                        