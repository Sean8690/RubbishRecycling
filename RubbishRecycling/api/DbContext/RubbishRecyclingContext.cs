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

        public RubbishRecyclingContext(DbContextOptions<RubbishRecyclingContext> options) : base(options)
        { }

        public RubbishRecyclingContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
        }
    }
}
