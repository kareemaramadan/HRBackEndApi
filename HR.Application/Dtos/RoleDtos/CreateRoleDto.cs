using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.RoleDtos
{
    public class CreateRoleDto
    {
        [Required, StringLength(100)]
        public string? Name { get; set; }
        [Required, StringLength(250)]
        public string? Description { get; set; }

    }
}
