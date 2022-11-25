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
        /// Returns all franchises with movie IDs
        /// </summary>
        /// <returns></returns>
        // GET: api/Franchises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadFranchiseDTO>>> GetFranchises()
        {
            var franchises =  _mapper.Map<List<ReadFranchiseDTO>>(await _context.Franchises
                .Include(fr => fr.Movies).ToListAsync());

            return franchises;
        }

        /// <summary>
        /// Get Franchise by ID
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <returns></returns>
        // GET: api/Franchises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadFranchiseDTO>> GetFranchise(int id)
        {
            var franchise = _mapper.Map<ReadFranchiseDTO>( await _context.Franchises.Include(fr=> fr.Movies).Where(fr => fr.Id == id).FirstOrDefaultAsync());

            if (franchise == null)
            {
                return NotFound();
            }

            return franchise;
        }

        /// <summary>
        /// Get movies from a Franchise
        /// </summary>
        /// <param name="id">Franchise Id</param>
        /// <returns>Movies from the selected franchise</returns>
        //GET: api/Franchises/Movies/5
        [HttpGet("Movies/{id}")]
        public async Task<ActionResult<List<ReadMovieDTO>>> GetFranchiseMovies(int id)
        {
            if (!FranchiseExists(id)) return NotFound();

           var movies = await _context.Movies.Where(m => m.FranchiseId == id).ToListAsync();

           return _mapper.Map<List<ReadMovieDTO>>(movies);
        }


        // PUT: api/Franchises/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFranchise(int id, Franchise franchise)
        {
            if (id != franchise.Id)
            {
                return BadRequest();
            }

            _context.Entry(franchise).State = EntityState.Modified;

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

        // POST: api/Franchises
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Franchise>> PostFranchise(Franchise franchise)
        {
            _context.Franchises.Add(franchise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFranchise", new { id = franchise.Id }, franchise);
        }

        // DELETE: api/Franchises/5
        [HttpDelete("{id}")]
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
