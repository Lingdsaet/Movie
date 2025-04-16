namespace Movie.Models
{
    public class Comment
    {
        
        public int CommentId { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Movie? Movie { get; set; }
        public Series? Series { get; set; }
        public User User { get; set; }
    }
}
