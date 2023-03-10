using System.ComponentModel.DataAnnotations;

namespace TheCompleteProject.ModelsAndDto_s.DbModels.Role.Dto
{
    public class AddRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}
