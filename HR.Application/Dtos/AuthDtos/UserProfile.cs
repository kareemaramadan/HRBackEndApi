using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class UserProfile
    {
        public string ?FirstName { get; set; }
        public string ?LastName { get; set; }
        public string ?Email { get; set; }
        public string ?NormalizedEmail { get; set; } 
        public string ?Username { get; set; } 
        public string ?NormalizedUsername { get; set; } 

        public string ?PhoneNumber { get; set; }

        public byte [ ]? ProfilePicture { get; set; }

    }
}
