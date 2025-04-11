
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Repository;
using Movie.RequestDTO;
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
        [HttpGet]
        public async Task<ActionResult<List<ActionContentDto>>> GetActionContent(
            [FromQuery] string sortBy = "title",
            [FromQuery] bool isDescending = false,
            [FromQuery] string search = ""
        )
        {
            var result = await _contentRepository.GetActionContentAsync(sortBy, isDescending, search);
            return Ok(result);
        }

    }
}
