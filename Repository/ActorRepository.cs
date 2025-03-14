using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly movieDB _context;

        public ActorRepository(movieDB context)
        {
            _context = context;
        }

        // Lấy thông tin diễn viên và các phim liên quan
        public async Task<ActorDetailDTO?> GetActorByIdAsync(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                    .ThenInclude(ma => ma.Movie)
                .Include(a => a.SeriesActors)
                    .ThenInclude(ma => ma.Series)
                .FirstOrDefaultAsync(a => a.ActorsId == id);

            if (actor == null) return null;

            var actorDetail = new ActorDetailDTO
            {
                Actor = new RequestActorDTO
                {
                    ActorsId = actor.ActorsId,
                    NameAct = actor.NameAct,
                    Nationality = actor.Nationality
                },
                Movie = actor.MovieActors.Select(ma => new ActorMovieDTO
                {
                    MovieId = ma.Movie.MovieId,
                    AvatarUrl = ma.Movie.AvatarUrl,
                    MovieName = ma.Movie.Title
                }).ToList(),
                Series = actor.SeriesActors.Select(ma => new ActorMovieDTO
                {
                    MovieId = ma.Series.SeriesId,
                    AvatarUrl = ma.Series.AvatarUrl,
                    MovieName = ma.Series.Title
                }).ToList()
            };

            return actorDetail;
        }

    }
}
