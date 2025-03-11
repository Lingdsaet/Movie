using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models
{
    public class MovieActor
    {
        public int MovieID { get; set; }
        public int ActorsID { get; set; }
        [ForeignKey("MovieID")]
        public required Movie Movie { get; set; }
        [ForeignKey("ActorsID")]
        public required Actor Actor { get; set; }
    }
}