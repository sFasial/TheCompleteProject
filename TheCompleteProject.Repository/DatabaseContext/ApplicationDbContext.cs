using Microsoft.EntityFrameworkCore;

namespace TheCompleteProject.Repository.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //DbSets Will Come Here
        public DbSet<> MyProperty { get; set; }
    }
}
