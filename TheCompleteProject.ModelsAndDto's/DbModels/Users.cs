﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheCompleteProject.ModelsAndDto_s.DbModels
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }

    public class GetUsersDto
    {
        public List<Users> Users { get; set; }
        public int TotalRecords { get; set; }
    }
}
