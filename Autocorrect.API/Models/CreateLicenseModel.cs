using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Models
{
    public class CreateLicenseModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaximumUtilizationCount { get; set; } = 1;
        public string Token { get; set; }
    }
}
