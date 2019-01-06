using Autocorrect.API.Data;
using Autocorrect.API.Data.DbEntities;
using Autocorrect.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Portable.Licensing;
using System;
using System.IO;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class LicenseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LicenseSettings _licenseSettings;

        public LicenseController(AppDbContext context, LicenseSettings licenseSettings)
        {
            _context = context;
            _licenseSettings = licenseSettings;
        }

        /// <summary>
        /// Used to create a new license
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("new")]
        public IActionResult New(CreateLicenseModel input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var licenseId = Guid.NewGuid();

            //create license
            var license = License.New()
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
                Status = Enums.LicenseStatus.Valid
            };
            _context.Licenses.Add(dbLicense);
            _context.SaveChanges();

            var licenseFile = new MemoryStream();
            license.Save(licenseFile);
            return File(licenseFile.ToArray(), "application/x-enterlicense", "License.lic");
        }

        /// <summary>
        /// Used to check if license has no been used mor then it can be
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("isvalid/{licenseId}")]
        public IActionResult IsLicenseValid(Guid licenseId)
        {
            var license = _context.Licenses.Find(licenseId);
            if (license == null) return Ok(false);

            return Ok(license.MaxUtilization != license.Utilized);

        }

        /// <summary>
        /// Called when license is activated to set the increated the utilized count so no licensei s used more then it should
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setusage/{licenseId}")]
        public IActionResult SetUssage(Guid licenseId)
        {
            var license = _context.Licenses.Find(licenseId);
            if (license == null) return NotFound();
            license.Utilized += 1;

            _context.SaveChanges();
            return Ok();

        }
    }
}
