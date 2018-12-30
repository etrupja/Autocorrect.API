using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Models
{
    public class SpecialWord
    {
        [Key]
        public string WrongWord { get; set; }
        public string RightWord { get; set; }
    }
}
