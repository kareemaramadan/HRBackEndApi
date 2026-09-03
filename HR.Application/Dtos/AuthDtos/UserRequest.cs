using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class UserRequest
    {
        public string id { get; set; } = Guid.NewGuid ( ).ToString ( );
        public string? userName { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? fullName { get; set; }
        public string? email { get; set; }
        public bool IsActivatedAccount { get; set; } = true;    
        public DateTime creationTime { get; set; }

    }
}
