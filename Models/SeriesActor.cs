using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("SeriesId", "ActorId")]
[Table("SeriesActor")]
public partial class SeriesActor
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [Key]
    [Column("ActorId")]
    public int ActorId { get; set; }

    [Column("SeriesActorID")]
    public int SeriesActorId { get; set; }

    [ForeignKey("ActorId")]
    [InverseProperty("SeriesActors")]
    public virtual Actors Actors { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("SeriesActors")]
    public virtual Series Series { get; set; } = null!;
}
