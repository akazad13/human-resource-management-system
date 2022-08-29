﻿using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.Auth
{
    public class AuthRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
