using System.Collections.Generic;
using System.Linq;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings.Dto;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Infrastructure;

namespace TheCompleteProject.Repository.Repositories.UserRoleMaping
{
    public interface IUserRoleMappingRepository:IBaseRepository<UserRoleMapping>
    {
        List<UserRoleMappingDto> GetAllRolesByUserId(int userId);
        IQueryable<object> GetUserRoleMapping();
    }

    public class UserRoleMappingRepository : BaseRepository<UserRoleMapping>, IUserRoleMappingRepository
    {
        public UserRoleMappingRepository(ApplicationDbContext context) : base(context)
        {

        }

        public List<UserRoleMappingDto> GetAllRolesByUserId(int userId)
        {
            var userRoles = (from a in _context.UserRoleMappings
                             join u in _context.Users on a.UserId equals u.Id
                             into uu
                             from uuu in uu.DefaultIfEmpty()
                             join r in _context.Roles on a.RoleId equals r.Id
                             into rr
                             from rrr in rr.DefaultIfEmpty()
                             where a.UserId == userId
                             select new UserRoleMappingDto
                             {
                                 UserId = uuu.Id,
                                 UserName = uuu.UserName,
                                 RoleId = rrr.Id,
                                 RoleName = rrr.RoleName,
                                 IsActive = a.IsActive
                             }).ToList();
            return userRoles;
        }

        public IQueryable<object> GetUserRoleMapping()
        {
            var userRoleMapping = (from a in _context.UserRoleMappings
                                   join u in _context.Users on a.UserId equals u.Id
                                   into uu
                                   from uuu in uu.DefaultIfEmpty()
                                   join r in _context.Roles on a.RoleId equals r.Id
                                   into rr
                                   from rrr in rr.DefaultIfEmpty()
                                   select new
                                   {
                                       UserId = uuu.Id,
                                       UserName = uuu.UserName,
                                       RoleId = rrr.Id,
                                       RoleName = rrr.RoleName,
                                       IsActive = a.IsActive
                                   });
            return userRoleMapping;
        }
    }
}
