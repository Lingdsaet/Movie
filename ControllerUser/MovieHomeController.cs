using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository;
using Movie.ResponseDTO;


namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieHomeController : ControllerBase
    {
        private readonly IMovieHome _movieHomeRepository;

        public MovieHomeController(IMovieHome movieHomeRepository)
        {
            _movieHomeRepository = movieHomeRepository;
        }
        
        [HttpGet("GetPosters")]
        public async Task<IActionResult> GetPosters()
        {
            var posters = await _movieHomeRepository.GetRandomPostersByIdAsync();
            return Ok(posters);
        }
        // Lấy danh sách phim mới
        [HttpGet("new")]
        public async Task<IActionResult> GetNewMovie()
        {
            var Movie = await _movieHomeRepository.GetNewMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim hot
        [HttpGet("MovieHot")]
        public async Task<IActionResult> GetHotMovie()
        {
            var Movie = await _movieHomeRepository.GetHotMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim bộ
        [HttpGet("SeriesHot")]
        public async Task<IActionResult> GetSeries()
        {
            var Movie = await _movieHomeRepository.GetSeriesHotAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim hành động
        [HttpGet("Action")]
        public async Task<IActionResult> GetActionMovie()
        {
            var Movie = await _movieHomeRepository.GetActionMovieAsync();
            return Ok(Movie);
        }

        [HttpGet("Anime")]
        public async Task<IActionResult> GetAnimeMovie()
        {
            var Movie = await _movieHomeRepository.GetAnimeSeriesAsync();
            return Ok(Movie);
        }
    }
}