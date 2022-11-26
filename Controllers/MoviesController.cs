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
using Assignment3MovieApi.DTOs.MovieDTOs;
using System.Data;
using Assignment3MovieApi.DTOs.CharacterDTOs;

namespace Assignment3MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;

        public MoviesController(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all movies in database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetMovies()
        {
            return _mapper.Map<List<Movie>, List<MovieReadDTO>>(await _context.Movies.Include(mo=> mo.Characters).ToListAsync());
        }

        /// <summary>
        /// Get a movie by Id
        /// </summary>
        /// <param name="id">Movie id as integer</param>
        /// <returns></returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieReadDTO>> GetMovie(int id)
        {
            var movie = _mapper.Map<MovieReadDTO>(await _context.Movies.Include(mo => mo.Characters).Where(mo => mo.Id == id).FirstOrDefaultAsync());

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }


        /// <summary>
        /// Gets Characters for a selected Movie
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <returns></returns>
        [HttpGet("{id}/characters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MovieCharacterReadDTO>>> GetMovieCharacters (int id)
        {
            if (!MovieExists(id)) return BadRequest();

            // Get all characters in a movie
            var movie = await _context.Movies.Include(m => m.Characters).Where(m => m.Id == id).FirstOrDefaultAsync();

            List<Character> domainCharacters = new List<Character>();
            foreach(var character in movie.Characters)
            {
                domainCharacters.Add(character);  
            }

            var characters = _mapper.Map<List<MovieCharacterReadDTO>>(domainCharacters);

            return characters;
           
        }

        /// <summary>
        /// Edit movie parameters
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <param name="movie">Full movie object in body</param>
        /// <returns></returns>
        /// 
        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMovie(int id, [FromBody] MovieUpdateDTO movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }
            var domainMovie = _mapper.Map<MovieUpdateDTO, Movie>(movie);

            _context.Entry(domainMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        /// Updates characters in a movie
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <param name="characterIds">Array of character Ids</param>
        /// <returns></returns>
        [HttpPut("{id}/characters")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCharactersInMovie (int id, int[] characterIds)
        {
            if (!MovieExists(id)) return BadRequest();

            var movie = await _context.Movies
                .Include(m => m.Characters)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            foreach (int charId in characterIds)
            {
                var character = await _context.Characters.FindAsync(charId);
                if(character == null)   return BadRequest("Character does not exist");
                
                movie.Characters.Add(character);
              
            }

            try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

            return NoContent();
        }


        /// <summary>
        /// Add a movie to the database
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieReadDTO>> PostMovie(MovieCreateDTO movie)
        {
            var domainMovie = _mapper.Map<MovieCreateDTO, Movie>(movie);

            _context.Movies.Add(domainMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = domainMovie.Id }, domainMovie);
        }

        /// <summary>
        /// Deletes movie from database
        /// </summary>
        /// <param name="id">Movie Id as integer</param>
        /// <returns></returns>
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

      
    }
}
