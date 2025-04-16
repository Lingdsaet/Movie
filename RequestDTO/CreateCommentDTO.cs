namespace Movie.RequestDTO
{
    public class CreateCommentMovieDTO
    {
        public int? MovieId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    public class CreateCommentSeriesDTO
    {
        public int? SeriesId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
