using Microsoft.AspNetCore.Mvc;
using Movies.Models;
using Movies.RequestDTO;
using Movies.ResponseDTO;

namespace Movies.Repository
{
    public interface IMovieRepository
    {
        Task<RequestMovieDTO> AddAsync(RequestMovieDTO movieDTO);
        Task<RequestMovieDTO?> UpdateAsync(RequestMovieDTO movieDTO);
        Task<RequestMovieDTO?> GetByIdAsync(int id);
        Task DeletedMoviesAsync(int id);
        Task<IEnumerable<RequestMovieDTO>> GetMoviesAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID);
        Task<RequestMovieDTO?> SoftDeleteAsync(int id);
        Task<IEnumerable<RequestMovieDTO>> GetDeleteAsync();

    }
}