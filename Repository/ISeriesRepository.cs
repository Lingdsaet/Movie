using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public interface ISeriesRepository 
    {
        Task<RequestSeriesDTO> GetSeriesByIdAsync(int id);
        Task<IEnumerable<Series>> GetAllAsync();
        Task<Series> GetByIdAsync(int id);
        Task AddAsync(Series entity);
        Task UpdateAsync(Series entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<RequestMovieDTO>> GetMovieAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID);
    }
}
