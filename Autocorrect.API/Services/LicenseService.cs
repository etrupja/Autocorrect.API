using Autocorrect.API.Data;
using Autocorrect.API.Data.DbEntities;
using Autocorrect.API.Models;
using Portable.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly AppDbContext _context;
        private readonly LicenseSettings _licenseSettings;

        public LicenseService(AppDbContext context, LicenseSettings licenseSettings)
        {
            _context = context;
            _licenseSettings = licenseSettings;
        }

        public Licenses CreateLicense(CreateLicenseModel input, Guid userId)
        {
            var licenseId = Guid.NewGuid();
            //create license
            var license = Portable.Licensing.License.New()
                .WithUniqueIdentifier(licenseId)
                .As(LicenseType.Standard)
                .ExpiresAt(DateTime.Now.AddYears(1))
                .WithMaximumUtilization(1)
                .LicensedTo(input.Name, input.Email)
                .CreateAndSignWithPrivateKey(_licenseSettings.PrivateKey, _licenseSettings.PassPhrase);

            //save license
            var dbLicense = new Licenses
            {
                Id = licenseId,
                MaxUtilization = input.MaximumUtilizationCount,
                ExpiresOn = DateTime.Now.AddYears(1),
                Status = Enums.LicenseStatus.Valid,
                UserId = userId
            };
            _context.SaveChanges();
            return dbLicense;
        }
    }
}
