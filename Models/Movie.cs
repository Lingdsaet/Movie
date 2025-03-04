using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models;

public partial class Movie
{
    [Key]
    [Column("MovieID")]
    public int MovieId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column("DirectorID")]
    public int? DirectorId { get; set; }

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

    [ForeignKey("DirectorId")]
    [InverseProperty("Movies")]
    public virtual Director? Director { get; set; }

    public int? Status { get; set; }

    [ForeignKey("MovieId")]
    [InverseProperty("Movies")]
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    [ForeignKey("MovieId")]
    [InverseProperty("Movies")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
