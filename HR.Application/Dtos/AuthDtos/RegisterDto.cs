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

        [Required, StringLength ( 11 )]
        public string ?PhoneNumber { get; set; }


        public byte [ ]? ProfilePicture { get; set; }

        public bool IsActivatedAccount { get; set; } = true;

        public DateTime CreatedOn = DateTime.Today;
    }
}
