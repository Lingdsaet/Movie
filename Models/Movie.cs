using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movie.Models;

namespace Movies.Models;

public partial class Movies
{
    [Key]
    [Column("MovieID")]
    public int MovieId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column("DirectorID")]
    public int? DirectorID { get; set; }

    [Column(TypeName = "decimal(3, 1)")]
    public decimal? Rating { get; set; }

    [Column("PosterURL")]
    [StringLength(255)]
    public string? PosterUrl { get; set; }

    [Column("AvatarURl")]
    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [Column("LinkFilmURl")]
    [StringLength(255)]
    public string? LinkFilmUrl { get; set; }

    [Column("IsHot")]
    public bool? IsHot { get; set; }

    [Column("YearReleased")]
    public int? YearReleased { get; set; }

    [ForeignKey("DirectorID")]
    [InverseProperty("Movies")]
    public virtual Directors? Director { get; set; }

    public int? Status { get; set; }


    [ForeignKey("MovieID")]
    [InverseProperty("Movies")]
    public virtual ICollection<Actors> Actors { get; set; } = new List<Actors>();

    [ForeignKey("MovieID")]
    [InverseProperty("Movies")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<MovieActors> MovieActor { get; set; } = new List<MovieActors>();
    public virtual ICollection<MovieCategory> MovieCategory { get; set; } = new List<MovieCategory>();
}