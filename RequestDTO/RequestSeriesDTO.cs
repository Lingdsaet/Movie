using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movies.Models;

public partial class Series
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column("DirectorID")]
    public int? DirectorId { get; set; }

    [Column(TypeName = "decimal(3, 1)")]
    public decimal? Rating { get; set; }

    public int Season { get; set; }

    [Column("PosterURL")]
    [StringLength(255)]
    public string? PosterUrl { get; set; }

    [Column("AvatarURl")]
    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [ForeignKey("DirectorId")]
    [InverseProperty("Series")]
    public virtual RequestDirectorDTO? Director { get; set; }

    [InverseProperty("Series")]
    public virtual ICollection<RequestEpisodeDTO> Episodes { get; set; } = new List<RequestEpisodeDTO>();

    [ForeignKey("SeriesId")]
    [InverseProperty("Series")]
    public virtual ICollection<RequestActorDTO> Actors { get; set; } = new List<RequestActorDTO>();

    [ForeignKey("SeriesId")]
    [InverseProperty("Series")]
    public virtual ICollection<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();
}
