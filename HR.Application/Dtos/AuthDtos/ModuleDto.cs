using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HR.Application.Dtos.AuthDtos
{
    public class ModuleDto
    {
        public int? ModuleId { get; set; }
        [Required]
        public string ModuleName_en { get; set; }   = string.Empty;
        [Required]
        public string ModuleName_ar { get; set; } = string.Empty;
        public byte[ ]? ModuleImage { get; set; }
    }

    public class CreateModuleDto
    {
        [Required]
        public string ModuleName_en { get; set; } = string.Empty;
        [Required]
        public string ModuleName_ar { get; set; } = string.Empty;
        public byte [ ]? ModuleImage { get; set; }
    }




}
