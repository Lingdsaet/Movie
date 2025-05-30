﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Repository;
using Movie.RequestDTO;

namespace Movie.Controllers
{
    [Route("api/AdminEpisode")]
    [ApiController]
    public class AdminEpisodeController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepo;
        private readonly movieDB _context;

        public AdminEpisodeController(IEpisodeRepository episodeRepo, movieDB context)
        {
            _episodeRepo = episodeRepo;
            _context = context;
        }

        // POST: api/AdminEpisode/AddEpisode
        [HttpPost("AddEpisode")]
        public async Task<IActionResult> AddEpisode([FromForm] RequestEpisodeDTO episodeDTO)
        {
            var result = await _episodeRepo.AddEpisodeAsync(episodeDTO);

            // Kiểm tra kết quả trả về từ repository
            if (result == null)
                return NotFound(new { Message = "Series không tồn tại." });

            return CreatedAtAction(nameof(GetEpisode), new { episodeId = result.EpisodeId }, result);
        }

        // GET: api/AdminEpisode/{episodeId}
        [HttpGet("{episodeId}")]
        public async Task<IActionResult> GetEpisode(int episodeId)
        {
            var episode = await _episodeRepo.GetByIdAsync(episodeId);
            if (episode == null)
            {
                return NotFound("Tập phim không tồn tại");
            }

            var episodeDTO = new RequestEpisodeDTO
            {
                EpisodeId = episode.EpisodeId,
                SeriesId = episode.SeriesId ?? 0,
                EpisodeNumber = episode.EpisodeNumber,
                Title = episode.Title,
                LinkFilmUrl = episode.LinkFilmUrl
            };

            return Ok(episodeDTO);
        }

        // GET: api/AdminEpisode/BySeries/{seriesId}
        [HttpGet("BySeries/{seriesId}")]
        public async Task<IActionResult> GetEpisodesBySeries(int seriesId)
        {
            var series = await _context.Series.FindAsync(seriesId);
            if (series == null)
            {
                return NotFound("Series không tồn tại");
            }

            var episodes = await _context.Episodes
                .Where(e => e.SeriesId == seriesId)   
                .Select(e => new RequestEpisodeDTO
                {
                    EpisodeId = e.EpisodeId,
                    SeriesId = e.SeriesId ?? 0,
                    EpisodeNumber = e.EpisodeNumber,
                    Title = e.Title,
                    LinkFilmUrl = e.LinkFilmUrl
                })
                .ToListAsync();

            return Ok(episodes);
        }

        // PUT: api/AdminEpisode/UpdateEpisode
        [HttpPut("UpdateEpisode")]
        public async Task<IActionResult> UpdateLink(int seriesId, int episodeNumber, [FromForm] string newLink)
        {
            var result = await _episodeRepo.UpdateLinkAsync(seriesId, episodeNumber, newLink);
            if (!result)
                return NotFound("Không tìm thấy tập phim cần cập nhật");

            return Ok("Cập nhật link thành công");
        }


        // DELETE: api/AdminEpisode/DeleteEpisode/{episodeId}
        [HttpDelete("DeleteEpisode/{episodeId}")]
        public async Task<IActionResult> DeleteEpisode(int episodeId)
        {
            var episode = await _episodeRepo.GetByIdAsync(episodeId);
            if (episode == null)
            {
                return NotFound("Tập phim không tồn tại");
            }

            await _episodeRepo.DeleteAsync(episodeId);

            return NoContent();
        }
    }
}