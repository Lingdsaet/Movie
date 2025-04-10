using Movie.Models;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public interface IUserUserRepository
    {
        Task<RequestUserDTO?> LoginAsync(string email, string password);
        Task<RequestUserDTO?> RegisterAsync(string username, string email, string password);
    }
}
