namespace Movies.RequestDTO
{
    public class RequestMovieActorDTO
    {
        public int MovieActorID { get; set; }
        public int MovieID { get; set; }
        public int ActorID { get; set; }
        public required RequestMovieDTO Movie { get; set; }
        public required RequestActorDTO Actor { get; set; }
    }
}
