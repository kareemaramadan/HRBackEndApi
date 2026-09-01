using HR.Application.Dtos.RoleDtos;
using HR.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
    public interface IRoleService
    {
        public Task<string> CreateRoleAsync(CreateRoleDto roleDto);
    }
}
