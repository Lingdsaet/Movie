
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Movie.Models;

namespace Movie.RequestDTO;

public partial class RequestMovieDTO
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? DirectorID { get; set; }

    public decimal? Rating { get; set; }
    public string? PosterUrl { get; set; }

    public string? AvatarUrl { get; set; }

    public string? LinkFilmUrl { get; set; }

    public string? Director { get; set; } = string.Empty!;

    public bool? IsHot { get; set; }

    public DateTime? YearReleased { get; set; }

    public int? Status { get; set; }
    public List<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();
    public List<RequestActorDTO> Actors {  get; set; } = new List<RequestActorDTO> { };
    public string? DirectorName { get; internal set; }
}
