using System;
using System.ComponentModel.DataAnnotations.Schema;
using Movie.Models;
using Movie.RequestDTO;

namespace Movie.ResponseDTO
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

        public int? YearReleased { get; set; }

    }
}
