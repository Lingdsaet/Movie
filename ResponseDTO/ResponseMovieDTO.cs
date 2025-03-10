using System.ComponentModel.DataAnnotations.Schema;
using Movies.Models;
using Movies.RequestDTO;

namespace Movies.ResponseDTO
{
    public class ResponseMovieDTO
    {
        public int MovieId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? DirectorID { get; set; }

        public decimal? Rating { get; set; }
        public string? PosterUrl { get; set; }

        public string? AvatarUrl { get; set; }

        public string? LinkFilmUrl { get; set; }

        public string Director { get; set; } = string.Empty!;

        public int? Status { get; set; }

        public bool? IsHot { get; set; }

        public DateTime? YearReleased { get; set; }

    }
}
