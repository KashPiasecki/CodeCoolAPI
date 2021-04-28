﻿using System.ComponentModel.DataAnnotations;

namespace CodeCoolAPI.Dtos
{
    public class IdentityRegistrationDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}