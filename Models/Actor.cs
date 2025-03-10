using Movies.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models;

public partial class Actor
{
    [Key]
    [Column("ActorsID")]
    public int ActorsID { get; set; }

    [StringLength(225)]
    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Nationality { get; set; }

    [StringLength(255)]
    public string? Professional { get; set; }

    [ForeignKey("ActorsID")]
    [InverseProperty("Actors")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    //[ForeignKey("ActorsID")]
    //[InverseProperty("Actors")]
    //public virtual ICollection<Series> Series { get; set; } = new List<Series>();
    public virtual ICollection<MovieActor> MovieActor { get; set; } = new List<MovieActor>();
}