namespace Movies.Models
{
    public class RequestMovieActorDTO
    {
        public int MovieID {  get; set; }
        public int ActorID { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}
