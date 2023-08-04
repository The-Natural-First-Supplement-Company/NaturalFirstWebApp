using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace NaturalFirstAPI.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        // Other DbSet properties for your application entities
    }
}
