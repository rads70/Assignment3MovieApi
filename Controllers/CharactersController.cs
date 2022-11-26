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
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharacters()
        {
            return _mapper.Map<List<CharacterReadDTO>>( await _context.Characters.Include(c=> c.Movies).ToListAsync());
        }

        /// <summary>
        /// Get character by Id
        /// </summary>
        /// <param name="id">Character Id</param>
        /// <returns></returns>
        // GET: api/Characters/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterReadDTO>> GetCharacter(int id)
        {
            var character = _mapper.Map<CharacterReadDTO>( await _context.Characters.Include(ch=> ch.Movies).Where(ch=> ch.Id == id).FirstOrDefaultAsync());

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        /// <summary>
        /// Update a character
        /// </summary>
        /// <param name="id"></param>
        /// <param name="character">full character object</param>
        /// <returns></returns>
        // PUT: api/Characters/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCharacter(int id, CharacterUpdateDTO character)
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

        /// <summary>
        /// Create a new character
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        // POST: api/Characters
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CharacterReadDTO>> PostCharacter(CharacterCreateDTO character)
        {
            var domainCharacter = _mapper.Map<Character>(character);
            _context.Characters.Add(domainCharacter);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter", new { id = domainCharacter.Id }, domainCharacter);
        }

        /// <summary>
        /// Delete a character
        /// </summary>
        /// <param name="id">Character Id</param>
        /// <returns></returns>
        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
