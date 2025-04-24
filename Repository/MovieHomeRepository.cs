using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public class MovieHomeRepository : IMovieHome
    {
        private readonly movieDB _context;
        public MovieHomeRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetRandomPostersByIdAsync()
        {
            var MovieIds = new List<int> { 1, 2, 3, 4, 5, 6 };
            var SeriesIds = new List<int> { 1, 2, 3, 4, 5, 6 };

            var moviePosters = await _context.Movies
                .Where(m => MovieIds.Contains(m.MovieId) && m.Status == 1 && !string.IsNullOrEmpty(m.PosterUrl))
                .Select(m => m.PosterUrl!)
                .ToListAsync();

            var seriesPosters = await _context.Series
                .Where(s => SeriesIds.Contains(s.SeriesId) && s.Status == 1 && !string.IsNullOrEmpty(s.PosterUrl))
                .Select(s => s.PosterUrl!)
                .ToListAsync();

            var random = new Random();
            var selectedMovies = moviePosters.OrderBy(_ => random.Next()).Take(3).ToList();
            var selectedSeries = seriesPosters.OrderBy(_ => random.Next()).Take(3).ToList();

            return selectedMovies.Concat(selectedSeries);
        }

        // home
        //public async Task<IEnumerable<string>> GetRandomPostersAsync()
        //{
        //    var moviePosters = await _context.Movies
        //        .Where(m => m.Status == 1 && m.PosterUrl != null)
        //        .Where(m => !string.IsNullOrEmpty(m.PosterUrl))
        //        .Select(m => m.PosterUrl)
        //        .ToListAsync();

        //    var seriesPosters = await _context.Series
        //        .Where(s => s.Status == 1 && s.PosterUrl != null)
        //        .Where(s => !string.IsNullOrEmpty(s.PosterUrl))
        //        .Select(s => s.PosterUrl)
        //        .ToListAsync();

        //    // Gộp lại và lấy random 3 poster
        //    var allPosters = moviePosters.Concat(seriesPosters).ToList();
        //    var random = new Random();
        //    var result = allPosters.OrderBy(x => random.Next()).Take(3).ToList();

        //    return result;
        //}
        public async Task<IEnumerable<RequestMovieDTO>> GetNewMovieAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.YearReleased)
                .Take(10);

            return await query.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToListAsync();
        }

        public async Task<IEnumerable<RequestMovieDTO>> GetHotMovieAsync()
        {
            var query = await _context.Movies
                .Where(m => m.Status == 1 && m.IsHot == true)               
                .ToListAsync();

            var random = new Random();
            var result = query.OrderBy(x => random.Next()).Take(10).ToList();
            return result.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
                IsHot = true
            }).ToList();
        }

        public async Task<IEnumerable<RequestSeriesDTO>> GetSeriesHotAsync()
        {
            var query = await _context.Series
                .Where(s => s.Status == 1 && s.IsHot == true)
                .ToListAsync();

            var random = new Random();
            var result = query.OrderBy(x => random.Next()).Take(10).ToList();
            return result.Select(series => new RequestSeriesDTO
            {
                SeriesId = series.SeriesId,
                Title = series.Title,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
            }).ToList();
           
        }


        public async Task<IEnumerable<RequestMovieDTO>> GetActionMovieAsync()
        {
            var query = await _context.Movies
                .Where(m => m.Status == 1 && m.MovieCategories.Any(mc => mc.Categories.CategoryName == "Hành động"))
                .ToListAsync();

            var random = new Random();
            var result = query.OrderBy(x => random.Next()).Take(10).ToList();
           
            return result.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToList();
        }

        public async Task<IEnumerable<RequestSeriesDTO>> GetAnimeSeriesAsync()
        {
            var query = await _context.Series
                .Where(m => m.Status == 1 && m.SeriesCategories.Any(mc => mc.Categories.CategoryName == "Anime"))
                .Where(s => s.Status == 1 && s.IsHot == true)
                .ToListAsync();

            var random = new Random();
            var result = query.OrderBy(x => random.Next()).Take(10).ToList();
            return result.Select(series => new RequestSeriesDTO
            {
                SeriesId = series.SeriesId,
                Title = series.Title,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
            }).ToList();

        }
    }
}