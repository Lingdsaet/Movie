using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

public partial class Director
{
    [Key]
    [Column("DirectorId")]
    public int DirectorId { get; set; }

    [StringLength(225)]
    public string NameDir { get; set; } = null!;


    [StringLength(100)]
    public string? Nationality { get; set; }


    [InverseProperty("Director")]
    public virtual ICollection<Movie> Movie { get; set; } = new List<Movie>();

    [InverseProperty("Director")]
    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
