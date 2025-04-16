using Movie.Models;

namespace Movie.RequestDTO
{
    public class RequestCommentDTO
    {
        public int CommentId { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
