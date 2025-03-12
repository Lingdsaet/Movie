using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.Repository;
using Movies.RequestDTO;
using Movies.ResponseDTO;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieHomeController : ControllerBase
    {
        private readonly IMovieHome _movieRepository;

        public MovieHomeController(IMovieHome movieRepository)
        {
            _movieRepository = movieRepository;
        }
        // Lấy poster
        [HttpGet]
        public async Task<IActionResult> GetPosterAsync()
        {
            var movies = await _movieRepository.GetPostersAsync();
            return Ok(movies);
        }
        // Lấy danh sách phim mới
        [HttpGet("new")]
        public async Task<IActionResult> GetNewMovies()
        {
            var movies = await _movieRepository.GetNewMoviesAsync();
            return Ok(movies);
        }

        // Lấy danh sách phim hot
        [HttpGet("hot")]
        public async Task<IActionResult> GetHotMovies()
        {
            var movies = await _movieRepository.GetHotMoviesAsync();
            return Ok(movies);
        }

        // Lấy danh sách phim bộ
        [HttpGet("series")]
        public async Task<IActionResult> GetSeriesMovies()
        {
            var movies = await _movieRepository.GetSeriesMoviesAsync();
            return Ok(movies);
        }

        // Lấy danh sách phim hành động
        [HttpGet("action")]
        public async Task<IActionResult> GetActionMovies()
        {
            var movies = await _movieRepository.GetActionMoviesAsync();
            return Ok(movies);
        }
    }
}