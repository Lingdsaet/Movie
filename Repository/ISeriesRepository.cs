using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public interface ISeriesRepository 
    {
        Task<RequestSeriesDTO> GetSeriesByIdAsync(int id);
        Task<IEnumerable<RequestSeriesDTO>> GetSeriesAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID);

    }
}
