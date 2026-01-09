using ExcelToSQLiteConverter.Data;
using Microsoft.EntityFrameworkCore;

namespace Satera_Api.Data
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options)
        : DbContext(options), IAppDbContext
    {
        public DbSet<App_Category> App_Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App_Category>(entity => entity.HasKey(row => row.Id));
            base.OnModelCreating(modelBuilder);
        }

    }
}