using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Repository;
using Movie.RequestDTO;

namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repo;

        public CommentController(ICommentRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("CommentMovie")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentMovieDTO dto)
        {
            try
            {
                var result = await _repo.AddCommentMovieAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CommentSeries")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentSeriesDTO dto)
        {
            try
            {
                var result = await _repo.AddCommentSeriesAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovie(int movieId)
        {
            var comments = await _repo.GetByMovieIdAsync(movieId);
            return Ok(comments);
        }

        [HttpGet("series/{seriesId}")]
        public async Task<IActionResult> GetBySeries(int seriesId)
        {
            var comments = await _repo.GetBySeriesIdAsync(seriesId);
            return Ok(comments);
        }

        [HttpPost("like/{commentId}")]
        public async Task<IActionResult> Like(int commentId)
        {
            var success = await _repo.LikeAsync(commentId);
            return success ? Ok("Lai sừ") : NotFound("Không tìm thấy bình luận.");
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var success = await _repo.DeleteAsync(commentId);
            return success ? Ok("Xoá rồi đỡ quê hơn chưa 😏 ") : NotFound("Không tìm thấy bình luận.");
        }
    }

}

