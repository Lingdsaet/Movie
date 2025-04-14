using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.RequestDTO;

namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesRepository _seriesRepository;

        public SeriesController(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        [HttpGet("detailSeries/{id}")]
        public async Task<ActionResult<RequestSeriesDTO>> GetDetailMovie(int id)
        {
            var movie = await _seriesRepository.GetSeriesByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Không tìm thấy phim.");
            }

            return Ok(movie);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetSeries(

             int? categoryID = null,
             string sortBy = "Title",
             string search = ""
             )
        {
            var Series = await _seriesRepository.GetSeriesAsync( sortBy, search, categoryID);
            return Ok(Series);
        }

    }
}
