using Autocorrect.API.Data;
using Autocorrect.API.ResponseModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class SyncController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SyncController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SpecialWords
        [HttpGet]
        [Route("all/{licenseId}")]
        public IActionResult GetAllWords(Guid licenseId)
        {
            var license = _context.Licenses.Find(licenseId);
            if (license == null) return Unauthorized();
            if (license.Status != Enums.LicenseStatus.Valid) return Unauthorized();
            var words = _context.SpecialWords.Select(n => new SpecialWordResponse()
            {
                WrongWord = n.WrongWord,
                RightWord = n.RightWord
            });

            return Ok(words);
        }


    }
}
