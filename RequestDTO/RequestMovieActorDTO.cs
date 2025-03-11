namespace Movies.RequestDTO
{
    public class RequestMovieActorDTO
    {
        public int MovieID { get; set; }
        public int ActorsID { get; set; }
        public required RequestMovieDTO Movie { get; set; }
        public required RequestActorDTO Actor { get; set; }
    }
}
