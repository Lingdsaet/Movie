using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Repository
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly movieDB _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ISeriesCategoryRepository<SeriesCategories> _seriesCategoryRepo;
        private readonly ISeriesActorRepository<SeriesActors> _seriesActorRepo;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public SeriesRepository(movieDB context, IWebHostEnvironment environment, ISeriesCategoryRepository<SeriesCategories> seriesCategoryRepo, ISeriesActorRepository<SeriesActors> seriesActorRepo)
        {
            _environment = environment;
            _context = context;
            _seriesCategoryRepo = seriesCategoryRepo;
            _seriesActorRepo = seriesActorRepo;
        }

        // Lấy danh sách series với phân trang, tìm kiếm và sắp xếp
        public async Task<PaginatedList<RequestSeriesDTO>> GetSeriesAsync(
            string? search = null,
            string sortBy = "Title",          // Sắp xếp theo tên series mặc định
            string sortDirection = "asc"     // Hướng sắp xếp mặc định là tăng dần

        )
        {
            var query = _context.Series.AsQueryable();

            // Tìm kiếm theo tiêu đề
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Title.Contains(search));
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "rating" => query.OrderByDescending(s => s.Rating),
                    "yearreleased" => query.OrderByDescending(s => s.YearReleased),
                    _ => query.OrderByDescending(s => s.Title),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "rating" => query.OrderBy(s => s.Rating),
                    "yearreleased" => query.OrderBy(s => s.YearReleased),
                    _ => query.OrderBy(s => s.Title),
                };
            }

            // Lấy tổng số bản ghi để tính phân trang
            var totalRecords = await query.CountAsync();

            // Phân trang
            var series = await query
                                .Select(s => new RequestSeriesDTO
                                {
                                    SeriesId = s.SeriesId,
                                    Title = s.Title,
                                    Description = s.Description,
                                    Rating = s.Rating,
                                    IsHot = s.IsHot,
                                    YearReleased = s.YearReleased,
                                    PosterUrl = s.PosterUrl,
                                    AvatarUrl = s.AvatarUrl,
                                    DirectorId = s.DirectorId,
                                    Season = s.Season,
                                    Nation = s.Nation,
                                    Status = s.Status,
                                    Director = s.Director.NameDir, // Lấy tên của Director
                                    Actors = s.SeriesActors.Select(sa => new RequestActorDTO
                                    {
                                        ActorId = sa.ActorId,
                                        NameAct = sa.Actors.NameAct // Lấy tên của Actor
                                    }).ToList(),
                                    Categories = s.SeriesCategories.Select(sc => new RequestCategoryDTO
                                    {
                                        CategoryId = sc.CategoryId,
                                        CategoryName = sc.Categories.CategoryName // Lấy tên của Category
                                    }).ToList()
                                })
                                .ToListAsync();

            // Return Paginated List
            return new PaginatedList<RequestSeriesDTO>(
                series,
                totalRecords
            );
        }

        // Lấy thông tin series theo ID
        public async Task<RequestSeriesDTO?> AdminGetSeriesByIdAsync(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series == null) return null;

            return new RequestSeriesDTO
            {
                SeriesId = series.SeriesId,
                Title = series.Title,
                Description = series.Description,
                DirectorId = series.DirectorId,
                Rating = series.Rating,
                IsHot = series.IsHot ?? false, // Xử lý nullable
                YearReleased = series.YearReleased,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
                Status = series.Status ?? 0, // Xử lý nullable
                Season = series.Season
            };
        }

        // Lưu ảnh vào thư mục chỉ định
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return null;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "fir-9a230-firebase-adminsdk-ag0bw-44a83a0ff8.json");

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                });
            }

            var credential = GoogleCredential.FromFile(path);
            var storage = StorageClient.Create(credential);

            string bucketName = "fir-9a230.appspot.com";
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string objectName = $"{folderName}/{fileName}";

            using (var stream = file.OpenReadStream())
            {
                await storage.UploadObjectAsync(bucketName, objectName, file.ContentType, stream);
            }

            string publicUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
            return publicUrl;
        }


        public async Task<RequestSeriesDTO> AddSeriesAsync(RequestSeriesDTO seriesDTO, IFormFile posterFile, IFormFile AvatarUrlFile)
        {
            // 1. Upload ảnh poster & avatar
            var posterUrl = await SaveFileAsync(AvatarUrlFile, "Posters");
            var avatarUrl = await SaveFileAsync(AvatarUrlFile, "AvatarUrl");

            // 2. Tạo entity Series
            var series = new Models.Series
            {
                Title = seriesDTO.Title,
                Description = seriesDTO.Description,
                DirectorId = seriesDTO.DirectorId,
                Rating = seriesDTO.Rating,
                IsHot = seriesDTO.IsHot,
                YearReleased = seriesDTO.YearReleased,
                PosterUrl = posterUrl,
                AvatarUrl = avatarUrl,
                Status = 1,
                Nation = seriesDTO.Nation,
                Season = seriesDTO.Season ?? 1
            };

            // 3. Thêm series vào DB
            _context.Series.Add(series);
            await _context.SaveChangesAsync(); // Lấy SeriesId sau khi thêm

            // 4. Gán lại poster/avatar vào DTO để trả về
            seriesDTO.PosterUrl = posterUrl;
            seriesDTO.AvatarUrl = avatarUrl;

            

            //Xử lý thêm Category vào MovieCategory
            if (seriesDTO.CategoryIds != null && seriesDTO.CategoryIds.Any())
            {
                string[] categoryId = seriesDTO.CategoryIds.Split(',');

                foreach (var category in categoryId)
                {
                    _context.SeriesCategories.Add(new SeriesCategories
                    {
                        SeriesId = series.SeriesId,
                        CategoryId = Int32.Parse(category)
                    });
                }
            }

            //  Xử lý thêm Actor vào MovieActors
            if (seriesDTO.ActorIds != null && seriesDTO.ActorIds.Any())
            {
                string[] actorId = seriesDTO.ActorIds.Split(',');

                foreach (var actor in actorId)
                {
                    _context.SeriesActors.Add(new SeriesActors
                    {
                        SeriesId = series.SeriesId,
                        ActorId = Int32.Parse(actor)
                    });
                }
            }


            // 7. Lưu toàn bộ thay đổi
            await _context.SaveChangesAsync();


            return seriesDTO;
        }


        public async Task<RequestSeriesDTO?> UpdateAsync(int id, RequestSeriesDTO seriesDTO, IFormFile? posterFile, IFormFile? AvatarUrlFile)
        {
            var series = await _context.Series
                .Include(m => m.SeriesCategories)
                .Include(m => m.SeriesActors)
                .FirstOrDefaultAsync(m => m.SeriesId == id);

            if (series == null)
            {
                return null;
            }

            if (posterFile != null)
            {
                series.PosterUrl = await SaveFileAsync(posterFile, "Posters");
            }
            if (AvatarUrlFile != null)
            {
                series.AvatarUrl = await SaveFileAsync(AvatarUrlFile, "AvatarUrl");
            }

            series.Title = seriesDTO.Title;
            series.Description = seriesDTO.Description;
            series.Rating = seriesDTO.Rating;

            series.Nation = seriesDTO.Nation;
            series.DirectorId = seriesDTO.DirectorId;
            series.IsHot = seriesDTO.IsHot;
            series.YearReleased = seriesDTO.YearReleased;


            if (seriesDTO.CategoryIds != null && seriesDTO.CategoryIds.Any())
            {
                _context.SeriesCategories.RemoveRange(series.SeriesCategories);
                string[] categoryIds = seriesDTO.CategoryIds.Split(',');
                foreach (var categoryId in categoryIds)
                {
                    if (int.TryParse(categoryId, out int parsedCategoryId))
                    {
                        // Kiểm tra xem CategoryId có tồn tại trong bảng Categories không
                        if (await _context.Categories.AnyAsync(c => c.CategoryId == parsedCategoryId))
                        {
                            _context.SeriesCategories.Add(new SeriesCategories
                            {
                                SeriesId = series.SeriesId,
                                CategoryId = parsedCategoryId
                            });
                        }
                    }
                }
            }

            if (seriesDTO.ActorIds != null && seriesDTO.ActorIds.Any())
            {
                _context.SeriesActors.RemoveRange(series.SeriesActors);
                string[] actorIds = seriesDTO.ActorIds.Split(',');
                foreach (var actorId in actorIds)
                {
                    if (int.TryParse(actorId, out int parsedActorId))
                    {
                        // Kiểm tra xem ActorId có tồn tại trong bảng Actors không
                        if (await _context.Actors.AnyAsync(a => a.ActorId == parsedActorId))
                        {
                            _context.SeriesActors.Add(new SeriesActors
                            {
                                SeriesId = series.SeriesId,
                                ActorId = parsedActorId
                            });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            seriesDTO.PosterUrl = series.PosterUrl;
            seriesDTO.AvatarUrl = series.AvatarUrl;

            return seriesDTO;
        }





        // Xóa một bộ series hoàn toàn khỏi cơ sở dữ liệu
        public async Task DeleteSeriesAsync(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                _context.Series.Remove(series);
                await _context.SaveChangesAsync();
            }
        }

        // Xóa mềm một bộ series (đặt Status = 0)
        public async Task SoftDeleteSeriesAsync(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                series.Status = 0;  // Đặt Status = 0 khi xóa mềm
                _context.Series.Update(series);
                await _context.SaveChangesAsync();
            }
        }

        // Cập nhật trạng thái của series
        public async Task UpdateSeriesStatusAsync(int id, int status)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                series.Status = status;
                _context.Series.Update(series);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RequestSeriesDTO> GetSeriesByIdAsync(int id)
        {

            var series = await _context.Series
            .Include(s => s.SeriesActors)
                .ThenInclude(sa => sa.Actors)
            .Include(s => s.SeriesCategories)
                .ThenInclude(sc => sc.Categories)
            .Include(s => s.Director)
            .FirstOrDefaultAsync(s => s.SeriesId == id);

            if (series == null) return null!;

            var seriesDTO = new RequestSeriesDTO
            {
                SeriesId= series.SeriesId,
                Title = series.Title,
                YearReleased = series.YearReleased,
                Nation = series.Nation ?? string.Empty,
                Rating= series.Rating,
                Season= series.Season,
                IsHot= series.IsHot,
                Categories = series.SeriesCategories.Select(sa => new RequestCategoryDTO
                {
                    CategoryId = sa.Categories.CategoryId,
                    CategoryName = sa.Categories.CategoryName
                }).ToList(),

                Description = series.Description ?? string.Empty,
                Episode = await _context.Episodes
                    .Where(e => e.SeriesId == series.SeriesId)
                    .Select(e => new RequestEpisodeDTO
                    {
                        EpisodeId=e.EpisodeId,
                        Title= e.Title,
                        EpisodeNumber = e.EpisodeNumber,
                        LinkFilmUrl = e.LinkFilmUrl ?? string.Empty
                    }).ToListAsync(),
                Actors = series.SeriesActors.Select(sa => new RequestActorDTO
                {
                    ActorId = sa.ActorId,
                    NameAct = sa.Actors.NameAct
                }).ToList(),
                Director = series.Director?.NameDir ?? string.Empty
            };

            return seriesDTO;
        }


        public async Task<IEnumerable<RequestSeriesDTO>> GetSeriesAsync(string sortBy, string search, int? categoryID)
        {
            var query = _context.Series
                .Include(s => s.Director)  // Bao gồm thông tin về Director
                .Include(s => s.SeriesActors).ThenInclude(sa => sa.Actors)  // Bao gồm thông tin về Actor liên quan đến Series
                .Include(s => s.SeriesCategories).ThenInclude(sc => sc.Categories)  // Bao gồm thông tin về Categories liên quan đến Series
                .Where(s => s.Status == 1); // Lọc theo status

            // Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Title.Contains(search)); // Lọc theo tiêu đề
            }

            if (categoryID.HasValue)
            {
                query = query.Where(s => s.SeriesCategories.Any(sc => sc.CategoryId == categoryID.Value)); // Lọc theo CategoryId
            }

            // Sorting
            query = sortBy switch
            {    
                "Title" => query.OrderBy(s => s.Title),
                "Rating" => query.OrderByDescending(s => s.Rating),
                _ => query.OrderBy(s => s.Title)
            };

            var seriesList = await query
                .ToListAsync();

            return seriesList.Select(s => new RequestSeriesDTO
            {
                SeriesId = s.SeriesId,
                Title = s.Title,
                Rating = s.Rating,
                IsHot = s.IsHot,
                YearReleased = s.YearReleased,
                PosterUrl = s.PosterUrl,
                AvatarUrl = s.AvatarUrl,
                DirectorId = s.DirectorId,
                Season = s.Season,
                Nation = s.Nation,
                Status = s.Status,
                Director = s.Director?.NameDir, // Lấy tên của Director
                Actors = s.SeriesActors.Select(sa => new RequestActorDTO
                {
                    ActorId = sa.ActorId,
                    NameAct = sa.Actors.NameAct // Lấy tên của Actor
                }).ToList(),
                Categories = s.SeriesCategories.Select(sc => new RequestCategoryDTO
                {
                    CategoryId = sc.CategoryId,
                    CategoryName = sc.Categories.CategoryName // Lấy tên của Category
                }).ToList()
            }).ToList();
        }
    }
    
}