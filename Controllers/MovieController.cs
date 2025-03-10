
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
        // Lấy danh sách phim
        [HttpGet("GetMovies")]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetMovies()
        {
            var movies = await _movieRepository.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("GetHomeMovies")]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetHomeMovies()
        {
            var movies = await _movieRepository.GetHomeMovies();
            return Ok(movies);
        }

        //  Lấy phim theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestMovieDTO>> GetByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound(new { Message = "Phim không tồn tại!" });
            }
            return Ok(movie);
        }

        // Thêm phim mới
        //[HttpPost]
        ////[Route("ADD")]
        //public async Task<ActionResult<Movie>> AddMovie([FromBody] RequestMovieDTO request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest(new { Message = "Dữ liệu không hợp lệ!" });
        //    }

        //    var RequestMovieDTO = new RequestMovieDTO
        //    {
        //        Title = request.Title,
        //        Description = request.Description,
        //        DirectorID = request.DirectorID,
        //        Rating = request.Rating,
        //        PosterUrl = request.PosterUrl,
        //        AvatarUrl = request.AvatarUrl,
        //        LinkFilmUrl = request.LinkFilmUrl,
        //    };
            
        //    await _movieRepository.AddAsync(RequestMovieDTO);
        //    return CreatedAtAction(nameof(GetByIdAsync), new { id = RequestMovieDTO.MovieId }, RequestMovieDTO);
        //}

        // Cập nhật phim
        //[HttpPut("{id}")]
        ////[Route("update")]
        //public async Task<ActionResult> UpdateMovie(int id, [FromBody] RequestMovieDTO request)
        //{
        //    var movie = await _movieRepository.GetByIdAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound(new { Message = "Phim không tồn tại!" });
        //    }

        //    movie.Title = request.Title;
        //    movie.Description = request.Description;
        //    movie.DirectorID = request.DirectorID;
        //    movie.Rating = request.Rating;
        //    movie.PosterUrl = request.PosterUrl;
        //    movie.AvatarUrl = request.AvatarUrl;
        //    movie.LinkFilmUrl = request.LinkFilmUrl;

        //    await _movieRepository.UpdateAsync(movie);
        //    return Ok(new { Message = "Cập nhật phim thành công!" });
        //}

    }
}