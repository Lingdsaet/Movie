
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.RequestDTO;

public partial class RequestSeriesDTO
{
    public int SeriesId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? DirectorId { get; set; }
    public string? Nation { get; set; }
    public decimal? Rating { get; set; }
    public int Season { get; set; }
    public string? PosterUrl { get; set; }
    public string? LinkFilmUrl { get; set; }
    public string? AvatarUrl { get; set; }
    public int Status { get; internal set; }
    public int? YearReleased { get; set; }
    public string? Director { get; set; } = string.Empty!;
    public int? TotalEpisode { get; set; }
    public virtual ICollection<RequestEpisodeDTO> Episode { get; set; } = new List<RequestEpisodeDTO>();
    public virtual ICollection<RequestActorDTO> Actors { get; set; } = new List<RequestActorDTO>();
    public virtual ICollection<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();
}

