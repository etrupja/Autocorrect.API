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
        public int Id { get; set; }

        public string WordWrong { get; set; }
        public string WordRight { get; set; }
    }
}
