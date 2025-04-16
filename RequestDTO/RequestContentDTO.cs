namespace Movie.RequestDTO
{
    public class RequestContentDTO
    {
        public class ActionContentDto
        {
            public int MovieId { get; set; }
            public string Title { get; set; }
            public string AvatarUrl { get; set; }

        }

        public class AnimeContentDto
        {
            public int SeriesId { get; set; }
            public string Title { get; set; }
            public string AvatarUrl { get; set; }

        }


    }
}