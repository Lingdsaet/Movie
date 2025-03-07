using Movies.Models;
using Movies.RequestDTO;

namespace Movies.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<RequestMovieDTO>> GetAllAsync();
        Task<RequestMovieDTO> GetByIdAsync(int id);
        Task AddAsync(RequestMovieDTO movie);
        Task UpdateAsync(RequestMovieDTO movie);
        Task DeleteAsync(int id);

    }
}