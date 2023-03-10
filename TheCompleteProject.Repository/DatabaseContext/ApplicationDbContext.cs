using Microsoft.EntityFrameworkCore;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;
using TheCompleteProject.ModelsAndDto_s.DbModels.Role;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings;

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
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
