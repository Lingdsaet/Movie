using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IMovieRepository
    {
        Task<RequestMovieDTO> AddAsync(RequestMovieDTO movieDTO, RequestCategoryDTO categoryDTO, RequestActorDTO actorDTO, IFormFile posterFile, IFormFile avatarFile);
        Task<RequestMovieDTO?> UpdateAsync(RequestMovieDTO movieDTO);
        Task<RequestMovieDTO?> GetByIdAsync(int id);
        Task DeletedMovieAsync(int id);
        Task<IEnumerable<RequestMovieDTO>> GetMovieAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID);
        Task<RequestMovieDTO?> SoftDeleteAsync(int id);
        Task<IEnumerable<RequestMovieDTO>> GetDeleteAsync();
        Task<RequestMovieDTO> GetMovieByIdAsync(int id);

    }
}