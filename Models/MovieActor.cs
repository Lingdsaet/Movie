namespace Movies.Models
{
    public class MovieActor
    {
        public int MovieActorId { get; set; }
        public int MovieID {  get; set; }
        public int ActorID { get; set; }
        public required Movie Movie { get; set; }
        public required Actor Actor { get; set; }
    }
}
