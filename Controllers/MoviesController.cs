using Microsoft.AspNetCore.Mvc;
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
            private readonly IMovieRepository<Movie> _movieRepository;
            private readonly IMovieCategoryRepository<MovieCategory> _movieCategoryRepository;
            private readonly IMovieActorRepository<MovieActor> _movieActorRepository;

            public MovieController(IMovieRepository<Movie> movieRepository,
                                   IMovieCategoryRepository<MovieCategory> movieCategoryRepository,
                                   IMovieActorRepository<MovieActor> movieActorRepository)
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
            public async Task<ActionResult<Movie>> AddMovie(Movie movie)
            {
                await _movieRepository.AddAsync(movie);
                foreach (var category in movie.Categories)
                {
                    if (category != null)
                    {
                        await _movieCategoryRepository.AddAsync(new MovieCategory
                        {
                            MovieID = movie.MovieId,
                            CategoriesID = category.CategoriesId
                        });
                    }
                }
               
                foreach (var actor in movie.Actors)
                {
                    await _movieActorRepository.AddAsync(new MovieActor
                    {
                        MovieID = movie.MovieId,
                        ActorID = actor.ActorsId
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
                existingMovie.DirectorId = int.Parse(Console.ReadLine());

                Console.Write("Nhập đánh giá mới: ");
                existingMovie.Rating = decimal.Parse(Console.ReadLine());

                Console.Write("Nhập URL poster mới: ");
                existingMovie.PosterUrl = Console.ReadLine();

                Console.Write("Nhập URL avatar mới: ");
                existingMovie.AvatarUrl = Console.ReadLine();

                Console.Write("Nhập URL phim mới: ");
                existingMovie.LinkFilmUrl = Console.ReadLine();


                await _movieRepository.UpdateAsync(existingMovie);

                // Cập nhật danh sách thể loại
                Console.Write("Nhập danh sách ID thể loại mới (cách nhau bởi dấu phẩy): ");
                string[] categoryIds = Console.ReadLine().Split(',');
                //foreach (var categoryId in categoryIds)
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
                        ActorID = int.Parse(actorId)
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
