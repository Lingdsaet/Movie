using System.Collections.Generic;

namespace Movie.ResponseDTO
{
    public class HomeResponse
    {
        public IEnumerable<ResponseMovieDTO> PosterMovie { get; set; }
        public IEnumerable<ResponseMovieDTO> HotMovie { get; set; }
        public IEnumerable<ResponseMovieDTO> NewMovie { get; set; }
        public IEnumerable<ResponseMovieDTO> SeriesMovie { get; set; }
        public IEnumerable<ResponseMovieDTO> ActionMovie { get; set; }
    }
}
