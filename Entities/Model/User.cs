﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class User
    {
        [Column("UserId")]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Login { get; set; }
        [Required]
        [StringLength(8)]
        public string Password { get; set; }
        public string Token { get; set; }
        public bool IsDeleted { get; set; }
        public RoleEnum Role { get; set; } = RoleEnum.User;

    }
}
