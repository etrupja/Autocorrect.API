using Autocorrect.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Data.DbEntities
{
    public class Licenses
    {
        [Key]
        public Guid Id { get; set; }
        public int MaxUtilization { get; set; }
        public int Utilized { get; set; }
        public DateTime ExpiresOn { get; set; }
        public LicenseStatus Status { get; set; }
        public Guid UserId { get; set; }
        public byte[] LicenseFile { get; set; }
    }
}
