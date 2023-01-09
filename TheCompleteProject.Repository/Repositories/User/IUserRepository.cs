using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.Repository.DatabaseContext;

namespace TheCompleteProject.Repository.Repositories.User
{
    public interface IUserRepository : IBaseRepository<Users>
    {
    }

    public class UserRepository : BaseRepository<Users>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
