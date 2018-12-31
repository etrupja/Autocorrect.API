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

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class SpecialWordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpecialWordsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SpecialWords
        [HttpGet]
        public IEnumerable<SpecialWord> GetSpecialWords()
        {
            return _context.SpecialWords;
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

            _context.Entry(specialWord).State = EntityState.Modified;

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
    }
}