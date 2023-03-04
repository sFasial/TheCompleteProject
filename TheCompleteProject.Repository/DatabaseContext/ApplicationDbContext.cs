using Microsoft.EntityFrameworkCore;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;

namespace TheCompleteProject.Repository.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //DbSets Will Come Here
        public DbSet<Users> Users { get; set; }
        public DbSet<UserRefreshTokens> UserRefreshTokens { get; set; }
    }
}
