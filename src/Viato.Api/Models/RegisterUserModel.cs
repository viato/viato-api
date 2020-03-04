﻿using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
