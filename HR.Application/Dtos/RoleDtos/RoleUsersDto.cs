using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Dtos.RoleDtos
{
    public class RoleUsersDto
    {
        public bool IsSuccess { get; set; }
        public IList<string>? Users { get; set; }
        public string? ErrorMessage { get; set; } = null;
    }


    public class UserRolesDto
    {
        public bool IsSuccess { get; set; }
        public IList<string>? Roles { get; set; }
        public string? ErrorMessage { get; set; } = null;
    }


}
