using Microsoft.EntityFrameworkCore;
using TheCompleteProject.ModelsAndDto_s.DbModels;

namespace TheCompleteProject.Repository.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //DbSets Will Come Here
        public DbSet<Users> Users { get; set; }
    }
}
