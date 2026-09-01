using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.RoleDtos
{
    public class RoleDto
    {
      
        public string? Name { get; set; }
       
        public string? Description { get; set; }

        public string? Message { get; set; }
    }
}
