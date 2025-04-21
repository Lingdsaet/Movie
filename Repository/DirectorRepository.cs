using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Repository
{
    public class DirectorRepository : IDirectorsRepository
    {
        private readonly movieDB _context;
        private readonly IWebHostEnvironment _environment;

        public DirectorRepository(movieDB context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Lấy chi tiết đạo diễn theo ID, bao gồm các bộ phim và series mà đạo diễn tham gia
        public async Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movie)  // Bao gồm các bộ phim mà đạo diễn tham gia
                .Include(d => d.Series) // Bao gồm các series mà đạo diễn tham gia
                .FirstOrDefaultAsync(d => d.DirectorId == id);  // Lấy đạo diễn theo ID

            if (director == null) return null;

            var directorDetail = new DirectorDetailDTO
            {
                Director = new RequestDirectorDTO
                {
                    DirectorID = director.DirectorId,
                    NameDir = director.NameDir,
                    Nationality = director.Nationality,

                },
                Movies = director.Movie.Select(m => new DirectorMoviesDTO
                {
                    MovieId = m.MovieId,
                    AvatarUrl = m.AvatarUrl,
                    MovieName = m.Title
                }).ToList(),
                Series = director.Series.Select(s => new DirectorSeriesDTO
                {
                    SeriesId = s.SeriesId,
                    AvatarUrl = s.AvatarUrl,
                    SerieName = s.Title
                }).ToList()
            };

            return directorDetail;
        }



        // Lấy tất cả các Director với phân trang, tìm kiếm, và sắp xếp
        public async Task<IEnumerable<RequestDirectorDTO>> GetAllDirectorsAsync(
            string? search = null,
            string sortBy = "DirectorId",
            string sortDirection = "asc"
)
        {
            var query = _context.Directors.AsQueryable();

            // Tìm kiếm theo tên hoặc mô tả director
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.NameDir.Contains(search) || d.Nationality.Contains(search) );
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "directorid" => query.OrderByDescending(d => d.DirectorId),
                    "namedir" => query.OrderByDescending(d => d.NameDir),
                    "nationality" => query.OrderByDescending(d => d.Nationality),
                    _ => query.OrderByDescending(d => d.DirectorId),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "directorid" => query.OrderBy(d => d.DirectorId),
                    "namedir" => query.OrderBy(d => d.NameDir),
                    "nationality" => query.OrderBy(d => d.Nationality),
                    _ => query.OrderBy(d => d.DirectorId),
                };
            }


            // Chuyển đổi thành RequestDirectorDTO
            var directorDTOs = await query
                .Select(d => new RequestDirectorDTO
                {
                    DirectorID = d.DirectorId,
                    NameDir = d.NameDir,
                    Nationality = d.Nationality,

                })
                .ToListAsync();

            return directorDTOs;
        }

        


        // Thêm mới một đạo diễn
        public async Task<RequestDirectorDTO> AddDirectorAsync(RequestDirectorDTO directorDTO, IFormFile? AvatarUrlFile)
        {


            var director = new Models.Director
            {
                NameDir = directorDTO.NameDir,
                Nationality = directorDTO.Nationality,
                // Add other properties as needed
            };

            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            // Update DTO with the new ID and URL
            directorDTO.DirectorID = director.DirectorId;  // Assuming DirectorId is the PK

            return directorDTO;
        }

        // Sửa thông tin đạo diễn
        public async Task<RequestDirectorDTO?> UpdateDirectorAsync(int id, RequestDirectorDTO directorDTO, IFormFile? AvatarUrlFile)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null) return null;

            director.NameDir = directorDTO.NameDir;
            director.Nationality = directorDTO.Nationality;

            await _context.SaveChangesAsync();

            directorDTO.DirectorID = director.DirectorId;
            return directorDTO;
        }

        public async Task<bool> DeleteDirectorAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null) return false;

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}