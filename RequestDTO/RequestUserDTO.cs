
namespace Movie.RequestDTO;


public partial class RequestUserDTO
{

    public int UserId { get; set; }


    public string UserName { get; set; } = null!;


    public string Email { get; set; } = null!;


    public string Password { get; set; } = null!;


    public DateTime? Createdat { get; set; }


    public virtual ICollection<RequestPaymentDTO> Payments { get; set; } = new List<RequestPaymentDTO>();
}
public class LoginDTO
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
