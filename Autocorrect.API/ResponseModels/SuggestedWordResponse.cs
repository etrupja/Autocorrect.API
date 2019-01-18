using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.ResponseModels
{
    public class SuggestedWordResponse
    {
        public string Id { get; set; }
        public string WrongWord { get; set; }
        public string RightWord { get; set; }
    }
}
