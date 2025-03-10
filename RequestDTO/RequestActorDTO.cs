using Movies.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Movies.RequestDTO;

public partial class RequestActorDTO
{
   
    public int ActorsID { get; set; }

    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    public string? Nationality { get; set; }

    public string? Professional { get; set; }

    public virtual ICollection<RequestMovieDTO> Movies { get; set; } = new List<RequestMovieDTO>();

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();

}
