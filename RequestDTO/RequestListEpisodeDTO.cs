namespace Movie.RequestDTO
{
    public class RequestListEpisodeDTO
    {
        public virtual ICollection<RequestEpisodeDTO> Episode { get; set; } = new List<RequestEpisodeDTO>();
    }
}
