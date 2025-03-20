using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUserNameAsync(string username);
        Task<User> CreateUserAsync(User user);
    }

}