using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Repository.Repositories.User;

namespace TheCompleteProject.Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IUserRepository UserRepository { get; }
    }
}
