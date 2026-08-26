using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class RegisterDto
    {
        [Required, MaxLength(150)]
        public required string Username { get; set; }
        [Required, MaxLength(150)]
        public required string FirstName { get; set; }
        [Required, MaxLength(150)]
        public required string LastName { get; set; }
        [Required, StringLength(150)]
        public required string Email { get; set; }
        [Required, StringLength(80)]
        public required string Password { get; set; }

        public DateTime CreatedOn = DateTime.Now;
    }
}
