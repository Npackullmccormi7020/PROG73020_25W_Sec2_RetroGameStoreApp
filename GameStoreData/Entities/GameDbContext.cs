using GameStoreData.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreData
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options) { }

        public DbSet<Games> Games { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Rewards> Rewards { get; set; } = null!;
        public DbSet<ShippingInfo> Shippings { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Games>().HasData(
                new Games { Id = 1, Name = "Zelda Minish Cap", Genre = "Action", Platform = "Gameboy Advance", Description = "Zela 2D Adventure", Price = 29.99m, Stock = 100, ReleaseDate = new DateTime(2004, 11, 4) },
                new Games { Id = 2, Name = "Pokemon Emerald", Genre = "Adventure", Platform = "Gameboy Advance", Description = "Pokemon Catch Them ALL", Price = 49.99m, Stock = 50, ReleaseDate = new DateTime(2002, 2, 1) }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, UserId = 1, GameId = 1, Quantity = 1, OrderDate = new DateTime(2025, 3, 1) },
                new Order { OrderId = 2, UserId = 2, GameId = 2, Quantity = 2, OrderDate = new DateTime(2025, 3, 2) }
            );

            modelBuilder.Entity<Rewards>().HasData(
                new Rewards { RewardId = 1, UserId = 1, Points = 100 },
                new Rewards { RewardId = 2, UserId = 2, Points = 200 }
            );

            modelBuilder.Entity<ShippingInfo>().HasData(
                new ShippingInfo { ShippingId = 1, OrderId = 1, Address = "123 Main St", ShippedDate = new DateTime(2025, 3, 5) },
                new ShippingInfo { ShippingId = 2, OrderId = 2, Address = "456 Elm St", ShippedDate = new DateTime(2025, 3, 6) }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "Kat", Email = "Kat@example.com" },
                new User { UserId = 2, UserName = "Nick", Email = "Nick@example.com" }
            );
        }
    }
}
