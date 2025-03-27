using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("SeriesId", "CategoryId")]
public partial class SeriesCategory
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [Key]
    [Column("CategoryId")]
    public int CategoryId { get; set; }

    [Column("SeriesCategoryId")]
    public int SeriesCategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("SeriesCategories")]
    public virtual Category Categories { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("SeriesCategories")]
    public virtual Series Series { get; set; } = null!;
}
