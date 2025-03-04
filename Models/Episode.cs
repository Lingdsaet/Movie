using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models;

public partial class Episode
{
    [Key]
    [Column("EpisodeID")]
    public int EpisodeId { get; set; }

    [Column("SeriesID")]
    public int? SeriesId { get; set; }

    public int EpisodeNumber { get; set; }

    [StringLength(255)]
    public string? Title { get; set; }

    [Column("LinkFilmURL")]
    [StringLength(255)]
    public string LinkFilmUrl { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("Episodes")]
    public virtual Series? Series { get; set; }
}
