using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.RequestDTO;

 
namespace Movie.ControllerWeb
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserUserController : ControllerBase
    {
        private readonly IUserUserRepository _userRepository;

        public UserUserController(IUserUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/User/SignUp
        [HttpPost("SignUp-User")]
        public async Task<IActionResult> CreateUser([FromBody] RequestSignUpDTO user)
        {
            try
            {
                var result = await _userRepository.RegisterAsync(user.UserName, user.Email, user.Password);
                if (result == null)
                {
                    return BadRequest(new { message = "Email hoặc Username thằng khác đã dùng!" });
                }

                return Ok(new { message = " Đăng ký xong rồi cook đi" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo người dùng." });
            }

        }


        // POST: api/User/login
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] RequestLoginDTO user)
        {
            var result = await _userRepository.LoginAsync(user.Email, user.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu rồi cưng!" });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                result.UserId,
                result.UserName,
                result.Email,
                result.Password,
                result.CreatedDate
            });
        }
       
    }

}