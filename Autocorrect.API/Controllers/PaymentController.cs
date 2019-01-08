using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var customers = new CustomerService();
            var charges = new ChargeService();
            
            var customer = customers.Create(new CustomerCreateOptions
            {
                //Email = chargeModel.stripeEmail,
                //SourceToken = chargeModel.stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 500, //5$ = 500cents
                Description = "Sample Charge",
                Currency = "usd",
                CustomerId = customer.Id
            });

            return Ok(charge);
        }

        [Route("chargetest")]
        [HttpGet]
        public IActionResult ChargeTest()
        {
            return Ok("tested");
        }
    }

    public class ChargeModel
    {
        public object token;
        public string amount;
    }
}