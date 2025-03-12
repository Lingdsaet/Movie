
using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using Movies.Repository;
using Movies.RequestDTO;
using Nest;

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

        //  Lấy danh sách phim + phân trang+ lọc + sắp xếp
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


        //  Lấy thông tin phim theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestMovieDTO>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }
            return Ok(movie);
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
        [HttpDelete("de/{id}")]
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

        //Danh sách xoá
        [HttpGet("getDeleted")]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetDeletedMovies()
        {
            var deletedMovies = await _movieRepository.GetDeleteAsync();

            if (deletedMovies == null || !deletedMovies.Any())
            {
                return NotFound(new { message = "No deleted movies found!" });
            }

            return Ok(deletedMovies);
        }

        //  Xoá vĩnh viễn các phim có Status = 0
        [HttpDelete("deleted")]
        public async Task<IActionResult> DeletedMovies(int id)
        {
            await _movieRepository.DeletedMoviesAsync(id);
            return NoContent();
        }
    }
}