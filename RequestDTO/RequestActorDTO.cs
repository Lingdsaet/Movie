namespace Movies.RequestDTO;

public partial class RequestActorDTO
{
    public int ActorsId { get; set; }

    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    public string? Nationlity { get; set; }

    public string? Professional { get; set; }

}
