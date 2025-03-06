using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movies.Models;

[Index("CategoryName", Name = "UQ__Categori__8517B2E0BC8DDCCD", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("CategoriesID")]
    public int CategoriesId { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [ForeignKey("CategoriesId")]
    [InverseProperty("Categories")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    [ForeignKey("CategoriesId")]
    [InverseProperty("Categories")]
    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
    public object? MovieCategory { get; internal set; }
}
