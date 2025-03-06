
namespace Movies.RequestDTO;


public partial class RequestUserDTO
{

    public int UserId { get; set; }


    public string Username { get; set; } = null!;


    public string Email { get; set; } = null!;


    public string Password { get; set; } = null!;


    public DateTime? Createdat { get; set; }


    public virtual ICollection<RequestPaymentDTO> Payments { get; set; } = new List<RequestPaymentDTO>();
}
