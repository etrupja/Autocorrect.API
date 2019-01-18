using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autocorrect.API.Enums;

namespace Autocorrect.API.Models
{
    public class SuggestedWord
    {
        [Key]
        public string Id { get; set; }
        public string WrongWord { get; set; }
        public string RightWord { get; set; }
        public WordStatus WordStatus { get; set; }
        public DateTime? DateSuggested { get; set; }
        public DateTime? DateAccepted { get; set; }
        public DateTime? DateRetreived { get; set; }
        public DateTime? DateRefused { get; set; }
    }
}
