using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public interface IEpisodeRepository
    {
        //Task AddAsync(Episode episode); // Thêm tập phim
        Task<RequestEpisodeDTO?> AddEpisodeAsync(RequestEpisodeDTO episodeDTO);
        Task<Episode> GetByIdAsync(int episodeId); // Lấy tập phim theo ID
        Task<List<Episode>> GetBySeriesIdAsync(int seriesId); // Lấy tất cả tập phim của series
        Task<bool> UpdateLinkAsync(int seriesId, int episodeNumber, string newLink);    
        Task DeleteAsync(int episodeId); // Xóa tập phim
        Task DeleteBySeriesIdAsync(int seriesId); // Xóa tất cả episode theo SeriesId
       
    }
}