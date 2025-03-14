using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IDirectorsRepository
    {
        Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id);
    }
}
