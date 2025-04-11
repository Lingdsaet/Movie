using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Repository;

namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IMovieRepository _movieRepository;

        public SearchController(ISeriesRepository seriesRepository, IMovieRepository movieRepository)
        {
            _seriesRepository = seriesRepository;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Mài nhập cl gì thế");
            var series = await _seriesRepository.SearchSeriesAsync(keyword);
            var movie = await _movieRepository.SearchMovieAsync(keyword);

            var result = new
            {
                Series = series.Select(s => new { s.SeriesId, s.Title, s.Rating, Type="Series" }),
                Movie = movie.Select(s => new { s.MovieId, s.Title, s.Rating, Type = "Series" }),

            };
            return Ok(result);
           
        }
        
    }
}
