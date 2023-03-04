using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheCompleteProject.ModelsAndDto_s.DbModels.Jwt
{
	[Table("UserRefreshTokens")]
	public class UserRefreshTokens
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public string RefreshToken { get; set; }
		public bool IsActive { get; set; } = true;
	}
}
