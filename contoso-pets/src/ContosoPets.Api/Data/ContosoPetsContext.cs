namespace ContosoPets.Api.Data
{
    using Microsoft.EntityFrameworkCore;
    using ContosoPets.Api.Models;
    
    public class ContosoPetsContext : DbContext
    {
        public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}