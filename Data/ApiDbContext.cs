namespace spatial_inventory_server.Data
{
    using Microsoft.EntityFrameworkCore;
    using spatial_inventory_server.Models;
    using System.Collections.Generic;

    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserLimits> UsersLimits { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Category -> User relationship
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.userId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete


            // Configure Product -> User relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.userId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete


            // Configure Product -> Category relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.categoryId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete


            // Configure UserLimits -> User relationship
            modelBuilder.Entity<UserLimits>()
                .HasOne(ul => ul.User)
                .WithOne(u => u.UserLimits)
                .HasForeignKey<UserLimits>(ul => ul.userId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Cascade delete if user is deleted


            base.OnModelCreating(modelBuilder);
        }
    }
}
