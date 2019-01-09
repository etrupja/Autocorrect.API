using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autocorrect.API.ResponseModels;
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
        [Route("charge")]
        [HttpPost]
        public IActionResult Charge(object token)
        {
            string tokenString = token.ToString();
            TokenResponse tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(tokenString);

            var customers = new CustomerService();
            var charges = new ChargeService();
            try
            {
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = tokenResponse.token.email,
                    SourceToken = tokenResponse.token.id
                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = tokenResponse.token.price * 100, //5$ = 500cents
                    Description = "Licensa TeksSakte",
                    Currency = "all",
                    CustomerId = customer.Id
                });

                return Ok(charge);
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