using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR.Application.Dtos.RoleDtos
{
    public class UpdateRoleDto
    {

        public string CurrentRoleName { get; set; } = string.Empty;

        public string NewName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public bool IsActive { get; set; } = true;
        [NotMapped]
        public bool IsDeleted { get; set; } = false;
        [NotMapped]
        public DateTime DeletedAt { get; set; }
    
    }
}
