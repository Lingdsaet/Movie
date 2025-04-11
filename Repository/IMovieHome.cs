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
        Task<IEnumerable<RequestSeriesDTO>> GetSeriesAsync();
        Task<IEnumerable<RequestMovieDTO>> GetActionMovieAsync();
    }
}
