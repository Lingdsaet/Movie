using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IActorRepository
    {
        Task<IEnumerable<RequestActorDTO>> GetActorsAsync(
        string? search = null,           // Tìm kiếm theo tên hoặc mô tả actor
        string sortBy = "ActorId",       // Sắp xếp theo tên actor mặc định
        string sortDirection = "asc"    // Hướng sắp xếp mặc định là tăng dần

        );
        Task<RequestActorDTO?> AdminGetActorByIdAsync(int id);
        Task<RequestActorDTO> AddActorAsync(RequestActorDTO actorDTO, IFormFile? AvatarUrlFile);
        Task<RequestActorDTO?> UpdateActorAsync(int id, RequestActorDTO actorDTO, IFormFile? AvatarUrlFile);
        Task<bool> DeleteActorAsync(int id);
        Task<string> SaveFile(IFormFile file, string folderName);  // Phương thức mới
        Task<ActorDetailDTO?> GetActorByIdAsync(int id);
    }
}