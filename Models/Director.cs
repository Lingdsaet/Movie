using Movies.Models;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models;

public partial class Director
{
    [Key]
    [Column("DirectorID")]
    public int DirectorID { get; set; }

    [StringLength(225)]
    public string NameDir { get; set; } = null!;
        
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Nationality { get; set; }

    [StringLength(255)]
    public string? Professional { get; set; }

    [InverseProperty("Director")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    //[InverseProperty("Director")]
    //public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}