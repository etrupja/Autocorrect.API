using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Models
{
    public class LicenseSettings
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string PassPhrase { get; set; }
        
    }
}
