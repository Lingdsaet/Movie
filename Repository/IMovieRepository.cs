using Movies.Models;
using Movies.RequestDTO;
using Movies.ResponseDTO;

namespace Movies.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<RequestMovieDTO>> GetAllAsync();
        Task<RequestMovieDTO> GetByIdAsync(int id);
        //Task AddAsync(RequestMovieDTO movie);
        //Task UpdateAsync(RequestMovieDTO movie);
        Task DeleteAsync(int id);
        Task<HomeResponse> GetHomeMovies();

    }
}