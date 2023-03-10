
namespace TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings.Dto
{
    public class UserRoleMappingDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddUserRoleMapping
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
