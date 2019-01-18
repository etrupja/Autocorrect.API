using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autocorrect.API.Data;
using Autocorrect.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Routing;
using Autocorrect.API.ResponseModels;
using Microsoft.AspNetCore.Authorization;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize]
    public class SuggestedWordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuggestedWordsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SuggestedWords
        [HttpGet]
        [Route("getallwords")]
        public IEnumerable<SuggestedWordResponse> GetAllWords()
        {
            List<SuggestedWordResponse> response = _context.SuggestedWords.Select(n => new SuggestedWordResponse()
            {
                Id = n.Id,
                WrongWord = n.WrongWord,
                RightWord = n.RightWord
            }).ToList();

            return response;
        }

        // POST: api/SuggestedWords
        [HttpPost]
        public async Task<IActionResult> PostSuggestedWord([FromBody] SuggestedWord suggestedWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (SuggestedWordExists(suggestedWord.WrongWord))
            {
                return BadRequest("Fjala existon");
            }
            else
            {
                suggestedWord.DateSuggested = DateTime.UtcNow.AddHours(2);
                _context.SuggestedWords.Add(suggestedWord);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        public bool SuggestedWordExists(string wrongWord) => _context.SuggestedWords.Any(n => n.WrongWord == wrongWord);
    }
}