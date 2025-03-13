using Movie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Movie.RequestDTO;

public partial class RequestActorDTO
{

    public int ActorsID { get; set; }

    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    public string? Nationality { get; set; }

    public string? Professional { get; set; }

    public virtual ICollection<RequestMovieDTO> Movie { get; set; } = new List<RequestMovieDTO>();

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();

}

public partial class ActorMovieDTO
{
    public int MovieId { get; set; }
    public string? AvatarUrl { get; set; }
    public string Tilte { get; set; }
}

public class ActorDetailDTO
{
    public RequestActorDTO Actor { get; set; }
    public List<ActorMovieDTO> Movie { get; set; }
}