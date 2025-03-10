namespace Movies.ResponseDTO
{
    public class HomeResponse
    {
        public IEnumerable<ResponseMovieDTO> PosterMovies { get; set; }
        public IEnumerable<ResponseMovieDTO> HotMovies { get; set; }
        public IEnumerable<ResponseMovieDTO> NewMovies { get; set; }
        public IEnumerable<ResponseMovieDTO> SeriesMovies { get; set; }
        public IEnumerable<ResponseMovieDTO> ActionMovies { get; set; }
    }
}
