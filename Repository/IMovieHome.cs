using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public interface IMovieHome
    {
        //Home
        Task<IEnumerable<string>> GetPostersAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetNewMovieAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetHotMovieAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetSeriesMovieAsync();
        Task<IEnumerable<ResponseMovieDTO>> GetActionMovieAsync();
    }
}
