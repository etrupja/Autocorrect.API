using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autocorrect.API.Data;
using Autocorrect.API.Models;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: api/SpecialWords/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialWord([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specialWord = _context.SpecialWords.FirstOrDefault(n => n.WordWrong.ToLower() == id);

            if (specialWord == null)
            {
                return NotFound();
            }

            return Ok(specialWord.WordRight);
        }

        // PUT: api/SpecialWords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialWord([FromRoute] int id, [FromBody] SpecialWord specialWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != specialWord.Id)
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
                if (!SpecialWordExists(id))
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

            _context.SpecialWords.Add(specialWord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecialWord", new { id = specialWord.Id }, specialWord);
        }

        // DELETE: api/SpecialWords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialWord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specialWord = await _context.SpecialWords.FindAsync(id);
            if (specialWord == null)
            {
                return NotFound();
            }

            _context.SpecialWords.Remove(specialWord);
            await _context.SaveChangesAsync();

            return Ok(specialWord);
        }

        private bool SpecialWordExists(int id)
        {
            return _context.SpecialWords.Any(e => e.Id == id);
        }
    }
}