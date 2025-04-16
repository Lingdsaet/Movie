
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Repository;
using NuGet.Protocol.Core.Types;
using static Movie.RequestDTO.RequestContentDTO;

namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ContentRepository _contentRepository;

        public ContentController(ContentRepository contentRepository)
        {
            _contentRepository = contentRepository;

        }
        [HttpGet("ActionFilm")]
        public async Task<ActionResult<List<ActionContentDto>>> GetActionContent(

            [FromQuery] string sortBy = "title",
            [FromQuery] bool isDescending = false,
            [FromQuery] string search = ""
)
        {
            var result = await _contentRepository.GetActionContentAsync(sortBy, isDescending, search);
            return Ok(result);
        }

        [HttpGet("HorrorFilm")]
        public async Task<ActionResult<List<ActionContentDto>>> GetHorrorContent(

            [FromQuery] string sortBy = "title",
            [FromQuery] bool isDescending = false,
            [FromQuery] string search = ""
)
        {
            var result = await _contentRepository.GetHorrorContentAsync(sortBy, isDescending, search);
            return Ok(result);

        }

        [HttpGet("AmimeFilm")]
        public async Task<ActionResult<List<ActionContentDto>>> GetAnimeContent(

            [FromQuery] string sortBy = "title",
            [FromQuery] bool isDescending = false,
            [FromQuery] string search = ""
)
        {
            var result = await _contentRepository.GetAnimeContentAsync(sortBy, isDescending, search);
            return Ok(result);

        }
    }
}