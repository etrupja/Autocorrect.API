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
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateRetreived { get; set; }
    }
}
