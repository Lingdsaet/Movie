using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class SeriesRepository : ISeries
    {
        private readonly movieDB _context;
        public SeriesRepository(movieDB context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Series>> GetAllAsync()
        {
            return await _context.Series.ToListAsync();
        }

        public Task<Series> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Series entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Series entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestSeriesDTO> GetSeriesByIdAsync(int id)
        {
            var movie = await _context.Series
                .Include(m => m.Director)
                .Include(m => m.SeriesActors)
                    .ThenInclude(ma => ma.Actors)
                .Include(m => m.SeriesCategories)
                    .ThenInclude(mc => mc.Categories)
                .FirstOrDefaultAsync(m => m.SeriesId == id);

            if (movie == null) return null!;

            var movieDTO = new RequestSeriesDTO
            {
                Title = movie.Title,
                LinkFilmUrl = movie.LinkFilmUrl,
                YearReleased = movie.YearReleased,
                Nation = movie.Nation,
                Categories = movie.MovieCategories
                    .Select(mc => new RequestCategoryDTO
                    {
                        CategoryName = mc.Categories.CategoryName
                    }).ToList(),
                Description = movie.Description,
                Actors = movie.MovieActors.Select(ma => new RequestActorDTO
                {
                    ActorsId = ma.ActorsId,
                    NameAct = ma.Actors.NameAct
                }).ToList(),
                Director = movie.Director!.NameDir
            };

            return movieDTO;
        }
    }
}