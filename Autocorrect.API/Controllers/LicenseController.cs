using Autocorrect.API.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Portable.Licensing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class LicenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LicenseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SpecialWords
        [HttpGet]
        public IActionResult Get()
        {
            var passPhrase = "Nertil";
            var keyGenerator = Portable.Licensing.Security.Cryptography.KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();
            var privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            var publicKey = keyPair.ToPublicKeyString();

            var license = License.New()
            .WithUniqueIdentifier(Guid.NewGuid())
            .As(LicenseType.Standard)
    .ExpiresAt(DateTime.Now.AddYears(1))
    .WithMaximumUtilization(1)
    .LicensedTo("Nertil Poci", "nertilpoci@hotmail.com")
    .WithAdditionalAttributes(x=>x.Add("MachineKey", "0f39c1df-2b70-4c65-a6c6-f9f2b540990e"))
    .CreateAndSignWithPrivateKey(privateKey, passPhrase);
            var licenseFile = new MemoryStream();
            license.Save(licenseFile);
            return File(licenseFile.ToArray(), "application/x-enterlicense", "License.lic");
        }

    }
}
