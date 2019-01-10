using Autocorrect.API.Data.DbEntities;
using Autocorrect.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Services
{
    public interface ILicenseService
    {
        Licenses CreateLicense(CreateLicenseModel input);
    }
}
