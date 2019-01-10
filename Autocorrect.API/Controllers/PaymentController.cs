using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autocorrect.API.Data;
using Autocorrect.API.Enums;
using Autocorrect.API.Models;
using Autocorrect.API.ResponseModels;
using Autocorrect.API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class PaymentController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        private readonly AppDbContext _context;
        public PaymentController(ILicenseService licenseService, AppDbContext context)
        {
            _licenseService = licenseService;
            _context = context;
        }


        [Route("charge")]
        [HttpPost]
        public IActionResult Charge(object token)
        {
            string tokenString = token.ToString();
            const int licensePrice = 2000;
            TokenResponse tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(tokenString);

            var customers = new CustomerService();
            var charges = new ChargeService();

            try
            {
                //create customer
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = tokenResponse.token.email,
                    SourceToken = tokenResponse.token.id
                });

                //create a charge/transaction
                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = tokenResponse.token.maximum_utilization_count * licensePrice * 100, //5$ = 500cents
                    Description = $"Licensa {tokenResponse.token.client_name} ({tokenResponse.token.client_email})",
                    Currency = "all",
                    CustomerId = customer.Id
                });
                
                //create a license
                var licenseModel = new CreateLicenseModel()
                {
                    Email = tokenResponse.token.email,
                    Name = tokenResponse.token.client_name,
                    MaximumUtilizationCount = tokenResponse.token.maximum_utilization_count
                };
                var newLicense = _licenseService.CreateLicense(licenseModel);
                newLicense.Status = (charge != null) ? LicenseStatus.Paid : LicenseStatus.Valid;
                
                _context.Licenses.Add(newLicense);
                _context.SaveChanges();
                return Ok("License u ble me sukses");
               
            }
            catch
            {
                return BadRequest("Pagesa nuk u procesua!");
            }
            
        }

        [Route("chargetest")]
        [HttpGet]
        public IActionResult ChargeTest()
        {
            return Ok("tested");
        }
    }
}