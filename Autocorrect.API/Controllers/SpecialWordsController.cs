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
    public class SpecialWordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpecialWordsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SpecialWords
        [HttpGet]
        [Route("getallwords")]
        public IEnumerable<SpecialWordResponse> GetAllWords()
        {
            List<SpecialWordResponse> response = _context.SpecialWords.Select(n => new SpecialWordResponse()
            {
                WrongWord = n.WrongWord,
                RightWord = n.RightWord
            }).ToList();

            return response;
        }

        [HttpGet]
        [Route("getupdatedwords")]
        public IEnumerable<SpecialWordResponse> GetUpdatedWords()
        {
            List<SpecialWordResponse> response = _context.SpecialWords
                .Where(n => n.DateUpdated != null)
                .Select(n => new SpecialWordResponse()
                {
                    WrongWord = n.WrongWord,
                    RightWord = n.RightWord
                }).ToList();

            return response;
        }

        [HttpGet]
        [Route("getnewwords")]
        public IEnumerable<SpecialWordResponse> GetNewWords()
        {
            var newWords = _context.SpecialWords.Where(n => n.DateRetreived == null);
            List<SpecialWordResponse> response = new List<SpecialWordResponse>();

            if (newWords != null)
            {
                foreach (SpecialWord newWord in newWords)
                {
                    //Set the Date the update was received
                    newWord.DateRetreived = DateTime.UtcNow.AddHours(2);

                    //Add the words to the response List
                    response.Add(new SpecialWordResponse()
                    {
                        WrongWord = newWord.WrongWord,
                        RightWord = newWord.RightWord
                    });
                }
                _context.SaveChanges();
            }

            return response;
        }

        // GET: api/SpecialWords/wrongWord
        [HttpGet("{wrongWord}")]
        public async Task<IActionResult> GetSpecialWord([FromRoute] string wrongWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specialWord = await _context.SpecialWords.FindAsync(wrongWord);

            if (specialWord == null)
            {
                return NotFound();
            }

            return Ok(specialWord);
        }

        // PUT: api/SpecialWords/wrongWord
        [HttpPut("{wrongWord}")]
        public async Task<IActionResult> PutSpecialWord([FromRoute] string wrongWord, [FromBody] SpecialWord specialWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (wrongWord != specialWord.WrongWord)
            {
                return BadRequest();
            }

            //Set DateRetreived to null
            SpecialWord oldSpecialWord = GetSingleSpecialWord(wrongWord);
            //oldSpecialWord.WrongWord = specialWord.WrongWord;
            oldSpecialWord.RightWord = specialWord.RightWord;
            oldSpecialWord.DateRetreived = null;
            oldSpecialWord.DateUpdated = DateTime.UtcNow.AddHours(2);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialWordExists(wrongWord))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SpecialWords
        [HttpPost]
        public async Task<IActionResult> PostSpecialWord([FromBody] SpecialWord specialWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (SpecialWordExists(specialWord.WrongWord))
            {
                return BadRequest("Fjala existon");
            }
            else
            {
                specialWord.DateAdded = DateTime.UtcNow.AddHours(2);
                _context.SpecialWords.Add(specialWord);
                await _context.SaveChangesAsync();
            }


            return CreatedAtAction("GetSpecialWord", new { wrongWord = specialWord.WrongWord }, specialWord);
        }

        // DELETE: api/SpecialWords/wrongWord
        [HttpDelete("{wrongWord}")]
        public async Task<IActionResult> DeleteSpecialWord([FromRoute] string wrongWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specialWord = await _context.SpecialWords.FindAsync(wrongWord);
            if (specialWord == null)
            {
                return NotFound();
            }

            _context.SpecialWords.Remove(specialWord);
            await _context.SaveChangesAsync();

            return Ok(specialWord);
        }

        private bool SpecialWordExists(string wrongWord)
        {
            return _context.SpecialWords.Any(e => e.WrongWord == wrongWord);
        }

        private SpecialWord GetSingleSpecialWord(string wrongWord)
        {
            return _context.SpecialWords.FirstOrDefault(e => e.WrongWord == wrongWord);
        }
    }
}