using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HR.Application.Dtos.AuthDtos
{
    public class AuthDto
    {
        public UserRequest User { get; set; }
        //public string? UserId { get; set; }
        public List<string>? Roles { get; set; }

        public string? Token { get; set; }
        public DateTime TokenExpiresOn { get; set; }

        
        public bool IsAuthenticated { get; set; } = false;

        public bool ISActivatedAccount { get; set; } = true;

        public string? Message { get; set; }
        
        
        
        //public string? Username { get; set; }
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        //public string? Email { get; set; }
        //public DateTime CreatedOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
