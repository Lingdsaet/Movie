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
        public async Task<ActionResult<RequestSeriesDTO>> GetDetailSeries(int id)
        {
            var series = await _seriesRepository.GetSeriesByIdAsync(id);
            if (series == null)
            {
                return NotFound("Không tìm thấy phim.");
            }

            return Ok(series);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestSeriesDTO>>> GetSeries(

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
