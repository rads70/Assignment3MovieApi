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
using Assignment3MovieApi.DTOs.FranchiseDTOs;
using Assignment3MovieApi.DTOs.MovieDTOs;

namespace Assignment3MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class FranchisesController : ControllerBase
    {
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;

        public FranchisesController(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all franchises in database
        /// </summary>
        /// <returns>All Franchsises in database</returns>
        /// <reponse code="200">Returns franchises in database</reponse>
        // GET: api/Franchises
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetFranchises()
        {
            var franchises =  _mapper.Map<List<FranchiseReadDTO>>(await _context.Franchises
                .Include(fr => fr.Movies).ToListAsync());

            return franchises;
        }

        /// <summary>
        /// Get Franchise by Id
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <returns>Franchise</returns>
        /// <reponse code="200">Returns franchise object</reponse>
        /// <reponse code="404">Franchise not found</reponse>
        // GET: api/Franchises/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchise(int id)
        {
            var franchise = _mapper.Map<FranchiseReadDTO>( await _context.Franchises.Include(fr=> fr.Movies).Where(fr => fr.Id == id).FirstOrDefaultAsync());

            if (franchise == null)
            {
                return NotFound();
            }

            return franchise;
        }

        /// <summary>
        /// Get all movies in a franchise
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <returns>Movies from the selected franchise</returns>
        /// <reponse code="200">Returns movies in selected franchise</reponse>
        //GET: api/Franchises/Movies/5
        [HttpGet("Movies/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MovieReadDTO>>> GetFranchiseMovies(int id)
        {
            if (!FranchiseExists(id)) return NotFound();
            var movies = _mapper.Map<List<MovieReadDTO>> (await _context.Movies.Include(mo=> mo.Characters).Where(mo => mo.FranchiseId == id).ToListAsync());

            return movies;

        }

        /// <summary>
        /// Get all characters in franchise
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <returns>List of characters</returns>
        /// <reponse code="200">Returns characters in the franchise</reponse>
        /// <reponse code="400">Franchise does not exist</reponse>
        [HttpGet("Characters/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MovieCharacterReadDTO>>> GetFranchiseCharacters(int id)
        {
            if(!FranchiseExists(id)) return BadRequest();

            var movies = await _context.Movies.Include(mo => mo.Characters).Where(mo => mo.FranchiseId == id).ToListAsync();

            List<Character> characters = new List<Character>();

            foreach (var mo in movies)
            {
                foreach (var character in mo.Characters)
                {
                    characters.Add(character);
                }
            }

            return _mapper.Map<List<MovieCharacterReadDTO>>(characters);


        }

        /// <summary>
        /// Create new franchise
        /// </summary>
        /// <param name="franchise">Franchise object</param>
        /// <returns>Created franchise</returns>
        /// <reponse code="201">Returns franchise object created</reponse>
        // POST: api/Franchises
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FranchiseReadDTO>> PostFranchise(FranchiseCreateDTO franchise)
        {
            var domainFranchise = _mapper.Map<Franchise>(franchise);

            _context.Franchises.Add(domainFranchise);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
           

            return CreatedAtAction("GetFranchise", new { id = domainFranchise.Id }, domainFranchise);
        }

        /// <summary>
        /// Update a franchise
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <param name="franchise">Complete franchise object</param>
        /// <returns></returns>
        /// <response code="204">Franchise updated - no content</response>
        /// <response code="400">id param doesn't match franchise object Id</response>
        /// <response code="404">Franchise not found</response>
        // PUT: api/Franchises/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutFranchise(int id, FranchiseUpdateDTO franchise)
        {
            if (id != franchise.Id)
            {
                return BadRequest();
            }

            var domainFranchise = _mapper.Map<Franchise>(franchise);

            _context.Entry(domainFranchise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FranchiseExists(id))
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
        /// Assign movies to a franchise
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <param name="movieIds">Array of movie Ids</param>
        /// <returns> No Content</returns>
        /// <response code="204">Movies assigned to franchise - no content</response>
        /// <response code="400">Franchise Id doesn't exist in database</response>
        /// <response code="404">Movie Id not found in database</response>
        [HttpPut("movies/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AssignMoviesToFranchise(int id, int[] movieIds)
        {
            if(!FranchiseExists(id)) return BadRequest();

            var franchise = await _context.Franchises
                .Include(fr => fr.Movies).Where(fr => fr.Id == id)
                .FirstOrDefaultAsync();

            foreach(int movId in movieIds)
            {
                var movie = await _context.Movies.FindAsync(movId);
                if (movie == null) return NotFound("Movie does not exist");

                franchise.Movies.Add(movie);
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
        /// Deletes a franchise by Id
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <returns></returns>
        /// <response code="204">Franchise deleted</response>
        /// <response code="404">Franchise not found</response>
        // DELETE: api/Franchises/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);
            if (franchise == null)
            {
                return NotFound();
            }

            _context.Franchises.Remove(franchise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FranchiseExists(int id)
        {
            return _context.Franchises.Any(e => e.Id == id);
        }
    }
}
