
using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using Movies.Repository;
using Movies.RequestDTO;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetMovies(
            int pageNumber = 1,
            int pageSize = 10,
            int? categoryID = null,
            string sortBy = "Title",
            string search = ""
            )
        {
            var movies = await _movieRepository.GetMoviesAsync(pageNumber, pageSize, sortBy, search, categoryID);
            return Ok(movies);
        }

        //  Thêm phim
        [HttpPost]
        public async Task<ActionResult<RequestMovieDTO>> AddMovie([FromBody] RequestMovieDTO request)
        {
            if (request == null)
            {
                return BadRequest("Invalid movie data.");
            }

            var movie = await _movieRepository.AddAsync(request);
            return CreatedAtAction(nameof(GetMovies), new { id = movie.MovieId }, movie);
        }

        // Sửa phim
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] RequestMovieDTO request)
        {
            if (request == null || id != request.MovieId)
            {
                return BadRequest("Invalid data");
            }

            var movie = await _movieRepository.UpdateAsync(request);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            return NoContent();
        }

        // Xoá mềm (chuyển status từ 1 -> 0)
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteMovie(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            await _movieRepository.SoftDeleteAsync(id);
            return NoContent();
        }

        //// Lịch sử xoá (phim có Status = 0)
        //[HttpGet("deleted")]
        //public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetDeletedMovies()
        //{
        //    var movies = await _movieRepository.SoftDeleteAsync();
        //    return Ok(movies);
        //}

        ////  Xoá vĩnh viễn các phim có Status = 0
        //[HttpDelete("deleted")]
        //public async Task<IActionResult> DeleteDeletedMovies()
        //{
        //    await _movieRepository.DeleteDeletedMoviesAsync();
        //    return NoContent();
        //}
    }
}