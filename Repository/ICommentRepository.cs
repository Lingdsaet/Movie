using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface ICommentRepository
    {
        Task<CreateCommentMovieDTO> AddCommentMovieAsync(CreateCommentMovieDTO dto);
        Task<CreateCommentSeriesDTO> AddCommentSeriesAsync(CreateCommentSeriesDTO dto);
        Task<IEnumerable<Comment>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<Comment>> GetBySeriesIdAsync(int seriesId);
        Task<bool> LikeAsync(int commentId);
        Task<bool> DeleteAsync(int commentId);

    }
}
