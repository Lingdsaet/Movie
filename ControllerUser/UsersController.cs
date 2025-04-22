using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.ResponseDTO;


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
        public async Task<IActionResult> CreateUser([FromBody] ResponseSignUpDTO user)
        {
            try
            {
                var result = await _userRepository.RegisterAsync(user.UserName, user.Email, user.Password);
                if (result == null)
                {
                    return BadRequest(new { message = "Email hoặc Username đã có người dùng bạn nhá. Đặt lại đi!" });
                }

                return Ok(new { message = " Đăng ký ngon lành rồi nha bạn! Đăng nhập thôi 😘 " });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Khoan! Đăng ký lỗi rồi" });
            }

        }


        // POST: api/User/login
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] ResponseLoginDTO user)
        {
            var result = await _userRepository.LoginAsync(user.Email, user.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu rồi cưng!😏😏" });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công! Xem phim thôi 😍😍😍😍",
                result.UserId,
                result.UserName,
                result.Email,
                result.Password,
                result.CreatedDate
            });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { Message = "Người dùng không tồn tại." });
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user.UserId, user.UserName, user.Email, hashedPassword);

            return Ok(new { Message = "Mật khẩu đã được cập nhật thành công." });
        }

    }

}