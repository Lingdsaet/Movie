
namespace Movie.RequestDTO;

public partial class RequestDirectorDTO
{
   
    public int DirectorID { get; set; }

    public string NameDir { get; set; } = null!;

    public string? Description { get; set; }


    public string? Nationality { get; set; }


    public string? Professional { get; set; }

}
