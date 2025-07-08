using MarketPriceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Data
{
    public class MarketPriceDbContext : DbContext
    {
        public MarketPriceDbContext(DbContextOptions<MarketPriceDbContext> options) : base(options) { }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<RealTimePriceSnapshot> RealTimePriceSnapshots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>().OwnsOne(a => a.Mappings, m =>
            {
                
                m.OwnsOne(x => x.Oanda, o =>
                {
                    o.Property(x => x.IsPresent).IsRequired();

                    o.Property(x => x.Symbol);
                    o.Property(x => x.Exchange);
                    o.Property(x => x.DefaultOrderSize);
                    o.Property(x => x.MaxOrderSize);

                    o.OwnsOne(x => x.TradingHours, th =>
                    {
                        th.Property(t => t.RegularStart);
                        th.Property(t => t.RegularEnd);
                        th.Property(t => t.ElectronicStart);
                        th.Property(t => t.ElectronicEnd);
                    });
                });

                
                m.OwnsOne(x => x.Simulation, s =>
                {
                    s.Property(x => x.IsPresent).IsRequired();

                    s.Property(x => x.Symbol);
                    s.Property(x => x.Exchange);
                    s.Property(x => x.DefaultOrderSize);
                    s.Property(x => x.MaxOrderSize);

                    s.OwnsOne(x => x.TradingHours, th =>
                    {
                        th.Property(t => t.RegularStart);
                        th.Property(t => t.RegularEnd);
                        th.Property(t => t.ElectronicStart);
                        th.Property(t => t.ElectronicEnd);
                    });
                });
            });

            modelBuilder.Entity<Asset>()
                .OwnsOne(a => a.Profile);

            modelBuilder.Entity<RealTimePriceSnapshot>()
                .HasIndex(r => r.instrumentId);
        }
    }
}
