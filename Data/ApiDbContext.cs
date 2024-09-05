namespace spatial_inventory_server.Data
{
    using Microsoft.EntityFrameworkCore;
    using spatial_inventory_server.Models;
    using System.Collections.Generic;

    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
