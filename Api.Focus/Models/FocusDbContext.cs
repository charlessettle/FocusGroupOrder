using System;
using Api.Focus.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;

namespace Api.Focus.Models
{
    public class FocusDbContext : DbContext
    {
        public FocusDbContext(DbContextOptions<FocusDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //#region composite primary keys
            //modelBuilder.Entity<LocalBusinessRating>()
            //    .HasKey(z => new { z.LocalBusinessId, z.UserId });

            //modelBuilder.Entity<BargainLike>()
            //    .HasKey(z => new { z.UserId, z.BargainId });

            //modelBuilder.Entity<BargainTag>()
            //    .HasKey(z => new { z.BargainId, z.TagId });
            //#endregion

            //#region cascading deletes [if we delete a bargain comment, we don't want to delete the bargain]
            //modelBuilder.Entity<BargainComment>()
            //    .HasOne(z => z.User)
            //    .WithMany(z => z.BargainComments)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<BargainLike>()
            //    .HasOne(z => z.User)
            //    .WithMany(z => z.BargainLikes)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<BargainTag>()
            //    .HasOne(z => z.Bargain)
            //    .WithMany(z => z.BargainTags)
            //    .OnDelete(DeleteBehavior.NoAction);
            //#endregion
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<LineItemPerUser> LineItemsPerUser { get; set; }
    }
}