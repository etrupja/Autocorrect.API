using Autocorrect.API.Data;
using Autocorrect.API.Data.DbEntities;
using Autocorrect.API.Models;
using Portable.Licensing;
using System;
using System.IO;

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
            var dbLicense = new Licenses
            {
                Id = licenseId,
                MaxUtilization = input.MaximumUtilizationCount,
                ExpiresOn = DateTime.Now.AddYears(1),
                Status = Enums.LicenseStatus.Valid,
                UserId = userId


            };
            using (var licenseStream = new MemoryStream())
            {
                license.Save(licenseStream);
                dbLicense.LicenseFile = licenseStream.ToArray();
            }
            _context.Licenses.Add(dbLicense);
            _context.SaveChanges();
            return dbLicense;
        }
    }
}
