namespace Movies.RequestDTO
{
    public class RequestMovieActorDTO
    {
        public int MovieID {  get; set; }
        public int ActorID { get; set; }
        public RequestMovieDTO Movie { get; set; }
        public RequestActorDTO Actor { get; set; }
    }
}
