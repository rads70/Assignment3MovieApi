using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment3MovieApi.Data;
using Assignment3MovieApi.Models;
using AutoMapper;
using Assignment3MovieApi.DTOs.CharacterDTOs;

namespace Assignment3MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;

        public CharactersController(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all characters from database
        /// </summary>
        /// <returns></returns>
        // GET: api/Characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCharacterDTO>>> GetCharacters()
        {
            return _mapper.Map<List<ReadCharacterDTO>>( await _context.Characters.Include(c=> c.Movies).ToListAsync());
        }

        /// <summary>
        /// Get character by Id
        /// </summary>
        /// <param name="id">Character Id</param>
        /// <returns></returns>
        // GET: api/Characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadCharacterDTO>> GetCharacter(int id)
        {
            var character = _mapper.Map<ReadCharacterDTO>( await _context.Characters.Include(ch=> ch.Movies).Where(ch=> ch.Id == id).FirstOrDefaultAsync());

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        // PUT: api/Characters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, Character character)
        {
            if (id != character.Id)
            {
                return BadRequest();
            }

            _context.Entry(character).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
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

        // POST: api/Characters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Character>> PostCharacter(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter", new { id = character.Id }, character);
        }

        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
