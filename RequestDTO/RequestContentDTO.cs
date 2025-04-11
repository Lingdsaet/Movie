namespace Movie.RequestDTO
{
    public class RequestContentDTO
    {
        public class ActionContentDto
        {
            public int SeriesId { get; set; }
            public int MovieId { get; set; }
            public string Title { get; set; }
            public string AvatarUrl { get; set; }
            public string Type { get; set; }
        }
        
    }
}
