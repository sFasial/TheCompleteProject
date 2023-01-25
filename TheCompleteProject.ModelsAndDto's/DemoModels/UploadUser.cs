using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCompleteProject.ModelsAndDto_s.DemoModels
{
    public class UploadUser
    {
        public IFormFile file { get; set; }
    }
    public class UploadUserErrorDto
    {
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();

    }

    public class UploadUserSuccessDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
