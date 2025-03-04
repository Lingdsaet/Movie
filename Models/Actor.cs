using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models;

public partial class Actor
{
    [Key]
    [Column("ActorsID")]
    public int ActorsId { get; set; }

    [StringLength(225)]
    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Nationlity { get; set; }

    [StringLength(255)]
    public string? Professional { get; set; }

    [ForeignKey("ActorsId")]
    [InverseProperty("Actors")]
    public virtual ICollection<Movies> Movies { get; set; } = new List<RequestMovieDTO>();

    [ForeignKey("ActorsId")]
    [InverseProperty("Actors")]
    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
