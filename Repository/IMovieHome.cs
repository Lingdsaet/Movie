using Movies.ResponseDTO;

namespace Movies.Repository
{
    public interface IMovieHome
    {
        //Home
        Task<IEnumerable<string>> GetPostersAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetNewMoviesAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetHotMoviesAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetSeriesMoviesAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetActionMoviesAsync();
    }
}
