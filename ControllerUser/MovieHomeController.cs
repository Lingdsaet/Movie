using Microsoft.AspNetCore.Mvc;
using Movie.Repository;


namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieHomeController : ControllerBase
    {
        private readonly IMovieHome _movieHomeRepository;

        public MovieHomeController(IMovieHome movieRepository)
        {
            _movieHomeRepository = movieRepository;
        }
        // Lấy poster
        [HttpGet("Poster")]
        public async Task<IActionResult> GetPosterAsync()
        {
            var poster = await _movieHomeRepository.GetRandomPostersAsync();
            return Ok(poster);
        }
        // Lấy danh sách phim mới
        [HttpGet("new")]
        public async Task<IActionResult> GetNewMovie()
        {
            var Movie = await _movieHomeRepository.GetNewMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim hot
        [HttpGet("hot")]
        public async Task<IActionResult> GetHotMovie()
        {
            var Movie = await _movieHomeRepository.GetHotMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim bộ
        [HttpGet("series")]
        public async Task<IActionResult> GetSeriesMovie()
        {
            var Series = await _movieHomeRepository.GetSeriesAsync();
            return Ok(Series);
        }

        // Lấy danh sách phim hành động
        [HttpGet("action")]
        public async Task<IActionResult> GetActionMovie()
        {
            var Movie = await _movieHomeRepository.GetActionMovieAsync();
            return Ok(Movie);
        }
    }
}