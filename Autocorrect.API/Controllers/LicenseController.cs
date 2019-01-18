using Autocorrect.API.Data;
using Autocorrect.API.Data.DbEntities;
using Autocorrect.API.Models;
using Autocorrect.API.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Portable.Licensing;
using Stripe;
using System;
using System.IO;
using System.Linq;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize]
    public class LicenseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LicenseSettings _licenseSettings;
        private readonly CustomerService _customerService;
        private readonly ChargeService _chargeService;
        private readonly ILicenseService _licenseService;
        public LicenseController(AppDbContext context, LicenseSettings licenseSettings, ILicenseService licenseService)
        {
            _context = context;
            _licenseSettings = licenseSettings;
            _customerService = new CustomerService();
            _chargeService = new ChargeService();
            _licenseService = licenseService;
        }


      
        [HttpGet]
        public IActionResult Get()
        {
            var userId = GetCurrentUser();
            var licenses = _context.Licenses.Where(z => z.UserId == userId);
            return Ok(licenses);
               
        }
        [Route("downloadlicense/{licenseId}")]
        [HttpGet]
        public IActionResult DownloadLicense(Guid licenseId)
        {
            var userId = GetCurrentUser();

            var license = _context.Licenses.SingleOrDefault(z=>z.Id==licenseId && z.UserId== userId);
            if (license == null) return NotFound();
            return File(license.LicenseFile, "application/x-enterlicense", "License.lic");

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
                var userId = GetCurrentUser();

                const int licensePrice = 2000;


                //create customer
                var customer = _customerService.Create(new CustomerCreateOptions
                {
                    Email = input.Email,
                    SourceToken = input.Token
                });

                //create a charge/transaction
                var charge = _chargeService.Create(new ChargeCreateOptions
                {
                    Amount = input.MaximumUtilizationCount * licensePrice * 100,// Better calculationis
                    Description = $"Pagese per {input.MaximumUtilizationCount} Licensa TekstSakte per {input.Name} ({input.Email})",
                    Currency = "all",
                    CustomerId = customer.Id
                });

                //check status
                if (!IsPaymentValid(charge)) return BadRequest(charge.FailureMessage);
                //create a license
                var licenseModel = new CreateLicenseModel()
                {
                    Email = input.Email,
                    Name = input.Name,
                    MaximumUtilizationCount = input.MaximumUtilizationCount
                };
                var newLicense = _licenseService.CreateLicense(licenseModel,GetCurrentUser());
                return Ok();
        }

        /// <summary>
        /// Used to check if license has no been used mor then it can be
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        [AllowAnonymous]
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
         [AllowAnonymous]
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
        private bool IsPaymentValid(Charge charge)
        {
            //Validate if payment is valid
            return charge!=null && charge.Paid;
        }
        private Guid GetCurrentUser()
        {
            var userid = HttpContext.User.FindFirst(JwtClaimTypes.Subject);
            if (userid == null) throw new Exception("Could not identify user");
             Guid.TryParse(userid.Value, out Guid uId);
            return uId;
        }
    }
}
