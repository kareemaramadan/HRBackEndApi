using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class ChangePasswordDto
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
