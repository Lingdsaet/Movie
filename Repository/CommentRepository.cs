
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly movieDB _context;

        public CommentRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<CreateCommentMovieDTO> AddCommentMovieAsync(CreateCommentMovieDTO dto)
        {
            var comment = new Comment
            {
                MovieId = dto.MovieId,
                UserId = dto.UserId,
                Content = dto.Content,
                CreatedDate = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var requestCommentDTO = new CreateCommentMovieDTO
            {

                MovieId = comment.MovieId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedDate = comment.CreatedDate,
            };

            return requestCommentDTO;
        }
        public async Task<CreateCommentSeriesDTO> AddCommentSeriesAsync(CreateCommentSeriesDTO dto)
        {
            var comment = new Comment
            {
                SeriesId = dto.SeriesId,
                UserId = dto.UserId,
                Content = dto.Content,
                CreatedDate = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            var requestCommentDTO = new CreateCommentSeriesDTO
            {
                SeriesId = comment.SeriesId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedDate = comment.CreatedDate,

            };

            return requestCommentDTO;
        }

        public async Task<IEnumerable<Comment>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Comments
                .Where(c => c.MovieId == movieId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetBySeriesIdAsync(int seriesId)
        {
            return await _context.Comments
                .Where(c => c.SeriesId == seriesId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> LikeAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            comment.Likes++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
       
    }

}

