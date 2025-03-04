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
    public virtual Director? Director { get; set; }

    [InverseProperty("Series")]
    public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();

    [ForeignKey("SeriesId")]
    [InverseProperty("Series")]
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    [ForeignKey("SeriesId")]
    [InverseProperty("Series")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
