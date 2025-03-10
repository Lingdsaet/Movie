using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models
{
    public class MovieActor
    {
        public int MovieActorID { get; set; }
        public int MovieID { get; set; }
        public int ActorsID { get; set; }
        [ForeignKey("MovieID")]
        public required Movie Movie { get; set; }
        [ForeignKey("ActorID")]
        public required Actor Actor { get; set; }
    }
}