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
    public string? Nationality { get; set; }

    [StringLength(255)]
    public string? Professional { get; set; }

    [ForeignKey("ActorsId")]
    [InverseProperty("Actors")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    [ForeignKey("ActorsId")]
    [InverseProperty("Actors")]
    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
