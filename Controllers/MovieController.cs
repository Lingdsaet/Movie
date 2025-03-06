//using Microsoft.AspNetCore.Mvc;
//using Movies.Models;
//using Microsoft.EntityFrameworkCore;
//using Movies.Repository;
//using Movies.RequestDTO;

//namespace Movies.Controllers
//{
//    namespace WebApplication3.Controllers
//    {
//        [Route("api/[controller]")]
//        [ApiController]
//        public class MovieController : ControllerBase
//        {
//            private readonly IMovieRepository<RequestMovieDTO> _movieRepository;
//            private readonly IMovieCategoryRepository<RequestMovieCategoryDTO> _movieCategoryRepository;
//            private readonly IMovieActorRepository<RequestMovieActorDTO> _movieActorRepository;

//            public MovieController(IMovieRepository<RequestMovieDTO> movieRepository)
//            {
//                _movieRepository = movieRepository;

//            }
//            [HttpGet]
//            public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetMovie(
//                string? search = null,  // Filtering by title
//                string? sortBy = "title", // Sorting field
//                string sortDirection = "asc", // Sorting direction
//                int page = 1, // Page number
//                int pageSize = 5 // Items per page
//                )
//            {

//                var movie = _movieRepository.GetAllAsync(search, sortBy, sortDirection, page, pageSize);
//                if (movie)
//                {
//                    return NotFound(new { Message = "No movies found" });
//                }

//                return Ok(new
//                {
//                    Movies = movie
//                });

//                return Ok(new
//                {
//                    TotalRecords = totalRecords,
//                    Page = page,
//                    PageSize = pageSize,
//                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
//                    Movies = movie
//                });
//            }

//            // 📌 Thêm phim mới với thể loại và diễn viên
//            //[HttpPost]
//            //[Route("InsertMovie")]
//            //public async Task<ActionResult<RequestMovieDTO>> AddMovie([FromBody] RequestMovieDTO request)
//            //{
//            //    if (request == null)
//            //    {
//            //        return BadRequest(new { message = "Dữ liệu không hợp lệ!" });
//            //    }

//            //    // Thêm movie vào database
//            //    var movie = new Movie
//            //    {
//            //        Title = request.Title,
//            //        Description = request.Description,
//            //        DirectorId = request.DirectorId,
//            //        Rating = request.Rating,
//            //        PosterUrl = request.PosterUrl,
//            //        AvatarUrl = request.AvatarUrl,
//            //        LinkFilmUrl = request.LinkFilmUrl,
//            //        Status = request.Status
//            //    };

//            //    await _movieRepository.AddAsync(movie);

//            //}
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using Movies.Repository;
using Movies.RequestDTO;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Lấy danh sách phim
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetMovies()
        {
            var movies = await _movieRepository.GetAllAsync();
            return Ok(movies);
        }

        //  Lấy phim theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestMovieDTO>> GetMovieById(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound(new { Message = "Phim không tồn tại!" });
            }
            return Ok(movie);
        }

        // Thêm phim mới
        [HttpPost]
        public async Task<ActionResult<Movie>> AddMovie([FromBody] RequestMovieDTO request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ!" });
            }

            var RequestMovieDTO = new RequestMovieDTO
            {
                Title = request.Title,
                Description = request.Description,
                DirectorId = request.DirectorId,
                Rating = request.Rating,
                PosterUrl = request.PosterUrl,
                AvatarUrl = request.AvatarUrl,
                LinkFilmUrl = request.LinkFilmUrl,
            };
            
            await _movieRepository.AddAsync(RequestMovieDTO);
            return CreatedAtAction(nameof(GetMovieById), new { id = RequestMovieDTO.MovieId }, RequestMovieDTO);
        }

        // Cập nhật phim
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromBody] RequestMovieDTO request)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound(new { Message = "Phim không tồn tại!" });
            }

            movie.Title = request.Title;
            movie.Description = request.Description;
            movie.DirectorId = request.DirectorId;
            movie.Rating = request.Rating;
            movie.PosterUrl = request.PosterUrl;
            movie.AvatarUrl = request.AvatarUrl;
            movie.LinkFilmUrl = request.LinkFilmUrl;

            await _movieRepository.UpdateAsync(movie);
            return Ok(new { Message = "Cập nhật phim thành công!" });
        }

        //// Xóa phim
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteMovie(int id)
        //{
        //    var movie = await _movieRepository.GetByIdAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound(new { Message = "Phim không tồn tại!" });
        //    }

        //    await _movieRepository.DeleteAsync(id);
        //    return Ok(new { Message = "Xóa phim thành công!" });
        //}
    }
}