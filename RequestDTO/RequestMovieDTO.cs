using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movies.RequestDTO;

public partial class RequestMovieDTO
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? DirectorId { get; set; }

    public decimal? Rating { get; set; }
    public string? PosterUrl { get; set; }

    public string? AvatarUrl { get; set; }

    public string? LinkFilmUrl { get; set; }

    public virtual Director? Director { get; set; }

    public int? Status { get; set; }
}
