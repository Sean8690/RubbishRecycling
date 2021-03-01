using Microsoft.EntityFrameworkCore;
using RubbishRecyclingAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU
{
    public class RubbishRecyclingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }

        public RubbishRecyclingContext(DbContextOptions<RubbishRecyclingContext> options) : base(options)
        { }

        public RubbishRecyclingContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(u => u.User)
            .HasForeignKey<Address>(u => u.UserId);

            modelBuilder.Entity<Product>()
            .HasOne(u => u.Address)
            .WithOne(u => u.Product)
            .HasForeignKey<Address>(u => u.ProductId);

            modelBuilder.Entity<Product>()
            .HasOne(u => u.User)
            .WithMany(u => u.Products)
            .HasForeignKey(u => u.UserId);
        }
    }
}
