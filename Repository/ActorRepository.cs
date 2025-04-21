using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Movie.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Movie.RequestDTO;
using System;

namespace Movie.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly movieDB _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public ActorRepository(movieDB context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Lấy tất cả các actor với phân trang, tìm kiếm, và sắp xếp
        public async Task<IEnumerable<RequestActorDTO>> GetActorsAsync(
            string? search = null,          // Tìm kiếm theo tên hoặc mô tả actor
            string sortBy = "ActorId",      // Sắp xếp theo tên actor mặc định
            string sortDirection = "asc"  // Hướng sắp xếp mặc định là tăng dần

        )
        {
            var query = _context.Actors.AsQueryable();

            // Tìm kiếm theo tên hoặc mô tả actor
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.NameAct.Contains(search));
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "actorid" => query.OrderByDescending(a => a.ActorId),
                    "nameact" => query.OrderByDescending(a => a.NameAct),
                    _ => query.OrderByDescending(a => a.ActorId),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "actorid" => query.OrderBy(a => a.ActorId),
                    "nameact" => query.OrderBy(a => a.NameAct),
                    _ => query.OrderBy(a => a.ActorId),
                };
            }

           

            // Chuyển đổi từ Actor sang RequestActorDTO
            var actors = await query.Select(a => new RequestActorDTO
            {
                ActorId = a.ActorId,
                NameAct = a.NameAct,

            }).ToListAsync();

            return actors;
        }

        // Lấy actor theo ID
        public async Task<RequestActorDTO?> AdminGetActorByIdAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return null;

            // Map Actor to RequestActorDTO
            return new RequestActorDTO
            {
                ActorId = actor.ActorId,
                NameAct = actor.NameAct,

            };
        }



        // Thêm mới một diễn viên
        public async Task<RequestActorDTO> AddActorAsync(RequestActorDTO actorDTO)
        {
           

            var actor = new Models.Actor
            {
                NameAct = actorDTO.NameAct,

            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            // Gán lại ID và AvatarUrl sau khi lưu
            actorDTO.ActorId = actor.ActorId;

            return actorDTO;
        }

        // Sửa thông tin diễn viên
        public async Task<RequestActorDTO?> UpdateActorAsync(int id, RequestActorDTO actorDTO)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return null;

           
            // Cập nhật các thông tin khác
            actor.NameAct = actorDTO.NameAct;

            await _context.SaveChangesAsync();

            actorDTO.ActorId = actor.ActorId;
            return actorDTO;
        }



        // Xóa actor theo ID
        public async Task<bool> DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return false;

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return true;
        }

      

        // Lấy thông tin diễn viên và các phim liên quan
        public async Task<ActorDetailDTO?> GetActorByIdAsync(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActor)
                    .ThenInclude(ma => ma.Movie)
                .Include(a => a.SeriesActors)
                    .ThenInclude(ma => ma.Series)
                .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor == null) return null;

            var actorDetail = new ActorDetailDTO
            {
                Actor = new RequestActorDTO
                {
                    ActorId = actor.ActorId,
                    NameAct = actor.NameAct,

                },
                Movies = actor.MovieActor.Select(ma => new ActorMoviesDTO
                {
                    MovieId = ma.Movie.MovieId,
                    AvatarUrl = ma.Movie.AvatarUrl,
                    MovieName = ma.Movie.Title
                }).ToList(),
                Series = actor.SeriesActors.Select(ma => new ActorSeriesDTO
                {
                    SeriesId = ma.Series.SeriesId,
                    AvatarUrl = ma.Series.AvatarUrl,
                    SerieName = ma.Series.Title
                }).ToList()
            };

            return actorDetail;
        }


    }
}