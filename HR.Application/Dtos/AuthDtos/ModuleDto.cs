using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }   = string.Empty;
        public byte[ ]? ModuleImage { get; set; }
    }
}
