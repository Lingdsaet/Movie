using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.RequestDTO;


namespace Movie.Repository
{
    public interface IMovieHome
    {
        //Home
        Task<IEnumerable<string>> GetRandomPostersAsync();
        Task<IEnumerable<RequestMovieDTO>> GetNewMovieAsync();
        Task<IEnumerable<RequestMovieDTO>> GetHotMovieAsync();
        Task<IEnumerable<RequestSeriesDTO>> GetSeriesHotAsync();
        Task<IEnumerable<RequestMovieDTO>> GetActionMovieAsync();
        Task<IEnumerable<RequestSeriesDTO>> GetAnimeSeriesAsync();
    }
}
