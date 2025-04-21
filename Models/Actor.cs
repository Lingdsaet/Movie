using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

public partial class Actor
{
    [Key]
    [Column("ActorId")]
    public int ActorId { get; set; }

    [StringLength(225)]
    public string NameAct { get; set; } = null!;


    [InverseProperty("Actors")]
    public virtual ICollection<MovieActors> MovieActor { get; set; } = new List<MovieActors>();

    [InverseProperty("Actors")]
    public virtual ICollection<SeriesActors> SeriesActors { get; set; } = new List<SeriesActors>();
}
