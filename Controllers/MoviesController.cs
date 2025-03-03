using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.Repository;

namespace Movies.Controllers
{
    namespace WebApplication3.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
         public class MovieController : ControllerBase
            {
                private readonly MovieRepository _movieRepository;
                private readonly MovieCategoryRepository _movieCategoryRepository;
                private readonly MovieActorRepository _movieActorRepository;

                public MovieController(MovieRepository movieRepository,
                                       MovieCategoryRepository movieCategoryRepository,
                                       MovieActorRepository movieActorRepository)
                {
                    _movieRepository = movieRepository;
                    _movieCategoryRepository = movieCategoryRepository;
                    _movieActorRepository = movieActorRepository;
                }
            //[HttpGet]
            //public async Task<ActionResult<IEnumerable<Movie>>> GetBooks(
            //    string? search = null,  // Filtering by title
            //    string? sortBy = "title", // Sorting field
            //    string sortDirection = "asc", // Sorting direction
            //    int page = 1, // Page number
            //    int pageSize = 5 // Items per page
            //    )
            //{
            //    if (page < 1) page = 1;
            //    if (pageSize < 1) pageSize = 10;

            //    var query = await _movieRepository.Where(b => b.Status == 1);

            //    // Filtering by title (case-insensitive)
            //    if (!string.IsNullOrWhiteSpace(search))
            //    {
            //        query = query.Where(b => b.Title.Contains(search));
            //    }

            //    // Sorting
            //    switch (sortBy.ToLower())
            //    {
            //        case "price":
            //            query = (sortDirection.ToLower() == "desc") ? query.OrderByDescending(b => b.Price) : query.OrderBy(b => b.Price);
            //            break;
            //        case "pubid":
            //            query = (sortDirection.ToLower() == "desc") ? query.OrderByDescending(b => b.PubId) : query.OrderBy(b => b.PubId);
            //            break;
            //        default: // Default sorting by title
            //            query = (sortDirection.ToLower() == "desc") ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title);
            //            break;
            //    }

            //    // Paging
            //    var totalRecords = await query.CountAsync();
            //    var books = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            //    if (!books.Any())
            //    {
            //        return NotFound(new { Message = "No books found" });
            //    }

            //    return Ok(new
            //    {
            //        TotalRecords = totalRecords,
            //        Page = page,
            //        PageSize = pageSize,
            //        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
            //        Books = books
            //    });
            //}
            
           

                // 📌 Thêm phim mới với thể loại và diễn viên
                [HttpPost]
                [Route("InsertMovie")]
                public async Task<ActionResult<Movie>> AddMovie()
                {
                    Console.Write("Nhập tiêu đề phim: ");
                    string title = Console.ReadLine();

                    Console.Write("Nhập mô tả: ");
                    string description = Console.ReadLine();

                    Console.Write("Nhập ID đạo diễn: ");
                    int directorId = int.Parse(Console.ReadLine());

                    Console.Write("Nhập đánh giá: ");
                    decimal rating = decimal.Parse(Console.ReadLine());

                    Console.Write("Nhập URL poster: ");
                    string posterUrl = Console.ReadLine();

                    Console.Write("Nhập URL avatar: ");
                    string avatarUrl = Console.ReadLine();

                    Console.Write("Nhập URL phim: ");
                    string linkFilmUrl = Console.ReadLine();

                    Console.Write("Nhập trạng thái: ");
                    int status = int.Parse(Console.ReadLine());

                    // Thêm movie vào database
                    Movie movie = new Movie
                    {
                        Title = title,
                        Description = description,
                        DirectorId = directorId,
                        Rating = rating,
                        PosterUrl = posterUrl,
                        AvatarUrl = avatarUrl,
                        LinkFilmUrl = linkFilmUrl,
                        Status = status
                    };

                    await _movieRepository.AddAsync(movie);

                    // Nhập danh sách thể loại
                    Console.Write("Nhập danh sách ID thể loại (cách nhau bởi dấu phẩy): ");
                    string[] categoryIds = Console.ReadLine().Split(',');
                    foreach (var categoryId in categoryIds)
                    {
                        await _movieCategoryRepository.AddAsync(new Movie
                        {
                            MovieId = movie.MovieId,
                            CategoriesId = int.Parse(categoryId)
                        });
                    }

                    // Nhập danh sách diễn viên
                    Console.Write("Nhập danh sách ID diễn viên (cách nhau bởi dấu phẩy): ");
                    string[] actorIds = Console.ReadLine().Split(',');
                    foreach (var actorId in actorIds)
                    {
                        await _movieActorRepository.AddAsync(new MovieActor
                        {
                            MovieID = movie.MovieID,
                            ActorsID = int.Parse(actorId)
                        });
                    }

                    return Ok(new { message = "Thêm phim thành công!", movie });
                }

                // 📌 Cập nhật phim với thể loại và diễn viên
                [HttpPost]
                [Route("UpdateMovie")]
                public async Task<ActionResult<Movie>> UpdateMovie()
                {
                    Console.Write("Nhập ID phim cần sửa: ");
                    int movieId = int.Parse(Console.ReadLine());

                    var existingMovie = await _movieRepository.GetByIdAsync(movieId);
                    if (existingMovie == null)
                    {
                        return NotFound(new { message = "Không tìm thấy phim!" });
                    }

                    Console.Write("Nhập tiêu đề mới: ");
                    existingMovie.Title = Console.ReadLine();

                    Console.Write("Nhập mô tả mới: ");
                    existingMovie.Description = Console.ReadLine();

                    Console.Write("Nhập ID đạo diễn mới: ");
                    existingMovie.DirectorID = int.Parse(Console.ReadLine());

                    Console.Write("Nhập đánh giá mới: ");
                    existingMovie.Rating = decimal.Parse(Console.ReadLine());

                    Console.Write("Nhập URL poster mới: ");
                    existingMovie.PosterURL = Console.ReadLine();

                    Console.Write("Nhập URL avatar mới: ");
                    existingMovie.AvatarURL = Console.ReadLine();

                    Console.Write("Nhập URL phim mới: ");
                    existingMovie.LinkFilmURL = Console.ReadLine();

                    Console.Write("Nhập trạng thái mới: ");
                    existingMovie.Status = int.Parse(Console.ReadLine());

                    await _movieRepository.UpdateAsync(existingMovie);

                    // Cập nhật danh sách thể loại
                    Console.Write("Nhập danh sách ID thể loại mới (cách nhau bởi dấu phẩy): ");
                    string[] categoryIds = Console.ReadLine().Split(',');

                    await _movieCategoryRepository.DeleteByMovieIdAsync(movieId);
                    foreach (var categoryId in categoryIds)
                    {
                        await _movieCategoryRepository.AddAsync(new MovieCategory
                        {
                            MovieID = movieId,
                            CategoriesID = int.Parse(categoryId)
                        });
                    }

                    // Cập nhật danh sách diễn viên
                    Console.Write("Nhập danh sách ID diễn viên mới (cách nhau bởi dấu phẩy): ");
                    string[] actorIds = Console.ReadLine().Split(',');

                    await _movieActorRepository.DeleteByMovieIdAsync(movieId);
                    foreach (var actorId in actorIds)
                    {
                        await _movieActorRepository.AddAsync(new MovieActor
                        {
                            MovieID = movieId,
                            ActorsID = int.Parse(actorId)
                        });
                    }

                    return Ok(new { message = "Cập nhật phim thành công!", existingMovie });
                }
            }
            //[HttpPost]
            //[Route("/InsertMovie")]
            //public async Task<ActionResult<Movie>> AddMovie(string title, string description, int directorId, decimal rating, string posterUrl, string avatarUrl, string linkFilmUrl, int status)
            //{
            //    Movie movie = new Movie
            //    {
            //        Title = title,
            //        Description = description,
            //        DirectorId = directorId,
            //        Rating = rating,
            //        PosterUrl = posterUrl,
            //        AvatarUrl = avatarUrl,
            //        LinkFilmUrl = linkFilmUrl,
            //        Status = status
            //    };

            //    await _movieRepository.AddAsync(movie);

            //    return Ok(new { movie });
            //}

            //[HttpPost]
            //[Route("/update")]
            //public async Task<ActionResult<Book>> UpdateBook(int book_id, string title, string type, decimal price, int pub_id, decimal advance, int royalty)
            //{
            //    //try
            //    //{
            //    var hh = await dbc.Books.FirstOrDefaultAsync(b => b.BookId == book_id);
            //    if (hh == null)
            //    { return NotFound(new { Message = "Book not found" }); }

            //    hh.Title = title;
            //    hh.Type = type;
            //    hh.Price = price;
            //    hh.Advance = advance;
            //    hh.Royalty = royalty;
            //    hh.PubId = pub_id;
            //    dbc.Books.Update(hh);
            //    await dbc.SaveChangesAsync();

            //    return Ok(hh);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($" error update book : {e.Message}");
            //    return StatusCode(500, "Error ");
            //}
        }
            ////DELETE: api/Books/5
            //[HttpDelete("{id}")]
            //public async Task<IActionResult> DeleteBook(int id)
            //{
            //    var book = await dbc.Books.FindAsync(id);
            //    if (book == null)
            //    {
            //        return NotFound();
            //    }

            //    dbc.Books.Remove(book);
            //    await dbc.SaveChangesAsync();

            //    return NoContent();
            //}

            //private bool BookExists(int id)
            ////{
            ////    return dbc.Books.Any(e => e.BookId == id);
            ////}

            //[HttpPut]
            //[Route("Status(delete)/{id}")]
            //public async Task<IActionResult> updatestatus(int id, int status)
            //{
            //    var hh = await dbc.Books.FirstOrDefaultAsync(b => b.BookId == id);
            //    if (hh == null)
            //    { return NotFound(new { Message = "Book not found" }); }

            //    if (status != 0 && status != 1)
            //    { return BadRequest(); }
            //    hh.Status = status;
            //    await dbc.SaveChangesAsync();

            //    return Ok(new { Message = "oke la" });

            //}
            //[HttpGet]
            //[Route("his.delete")]
            //public async Task<ActionResult<IEnumerable<Book>>> GetBooksF()
            //{
            //    var books = await dbc.Books.Where(b => b.Status == 0).ToListAsync();

            //    if (books.Count == 0)
            //    {
            //        return NotFound(new { Message = "No books found for deletion." });
            //    }

            //    return Ok(books);
            //}
        }
