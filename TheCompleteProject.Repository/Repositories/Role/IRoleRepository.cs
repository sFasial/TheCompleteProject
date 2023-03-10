using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.Role;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Infrastructure;

namespace TheCompleteProject.Repository.Repositories.Role
{
    public interface IRoleRepository : IBaseRepository<Roles>
    {
    } 
    
    public class RoleRepository: BaseRepository<Roles> , IRoleRepository 
    {
        public RoleRepository(ApplicationDbContext context) : base (context)
        {

        }
    }
}
