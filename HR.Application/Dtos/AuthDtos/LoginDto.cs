using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class LoginDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
